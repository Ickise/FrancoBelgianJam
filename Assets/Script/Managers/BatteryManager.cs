using System;
using UnityEngine;
using System.Collections;

public class BatteryManager : MonoBehaviour
{
    public static BatteryManager instance;

    [SerializeField, Header("References")] private UIBattery uiBattery;
    [SerializeField] private InputReader inputReader;

    [SerializeField, Header("Settings")] private float maxBattery = 100f;
    [SerializeField] private float moveConsumption = 1f;
    [SerializeField] private float vacuumConsumption = 2f;
    [SerializeField] private float makeNoiseConsumption = 2f;
    [SerializeField] private float gasIntoEnergyConversion = 2f;

    private float currentBattery;
    private float maxOvercharge;
    private float overchargeDepletionRate = 0.3f;
    private float gasConversionRate = 0.5f;
    private float actionConsumptionRate = 1f;

    private GameManager gameManager;

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
        maxOvercharge = maxBattery * 1.5f;
    }
    
    private void Start()
    {
        gameManager = GameManager.instance;
        uiBattery.UpdateEnergyUI();
    }

    private void ChangeEnergyValue(float amount, bool isIncreasing)
    {
        if (isIncreasing)
        {
            currentBattery += amount;
        }
        else
        {
            currentBattery -= amount;
        }

        currentBattery = Mathf.Clamp(currentBattery, 0, maxOvercharge);
        uiBattery.UpdateEnergyUI();

        if (currentBattery <= 0)
        {
            gameManager.GameOver();
        }
    }

    private void DepleteBattery()
    {
    }

    public void RechargeBattery(float gasAmount)
    {
        float energyGained = gasAmount * gasIntoEnergyConversion;
        ChangeEnergyValue(energyGained, true);
    }

    public float GetCurrentBattery()
    {
        return currentBattery;
    }

    public float GetMaxBattery()
    {
        return maxBattery;
    }

    private IEnumerator ConsumeBattery()
    {
        yield return null;
    }
}