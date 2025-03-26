using UnityEngine;
using System;

public class BatteryManager : MonoBehaviour
{
    public static BatteryManager instance;
    
    [SerializeField, Header("References")] private UIBattery uiBattery;

    [SerializeField, Header("Settings")] private float maxBattery = 100f;
    [SerializeField] private float moveConsumption = 1f;
    [SerializeField] private float vacuumConsumption = 2f;
    [SerializeField] private float makeNoiseConsumption = 2f;
    [SerializeField] private float gasIntoEnergyConversion = 2f;

    private GameManager gameManager;

    private float currentBattery;
    private float actionConsumptionRate = 1f;
    private float overchargeDepletionRate = .3f;
    private float gasConversionRate = .5f;
    private float maxOvercharge = 150f; // Pourcentage de 150%

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentBattery = maxBattery;
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        uiBattery.UpdateEnergyUI();
    }

    public void ChangeEnergyValue(int amount, bool isIncreasing)
    {
        EnergyIsSoldOut();
        currentBattery = isIncreasing ? currentBattery + amount : currentBattery - amount;
        currentBattery = Mathf.Clamp(currentBattery, 0, maxBattery);
        uiBattery.UpdateEnergyUI();
    }

    public void EnergyIsSoldOut()
    {
        if (currentBattery <= 0)
        {
            gameManager.GameOver();
        }
    }

    public float GetCurrentBattery()
    {
        return currentBattery;
    }

    public float GetMaxBattery()
    {
        return maxBattery;
    }
}