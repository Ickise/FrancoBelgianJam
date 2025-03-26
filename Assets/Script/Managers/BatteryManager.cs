using UnityEngine;

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

    private float actionTime;

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

    private void OnEnable()
    {
        inputReader.AnyTriggerHeld += () => ChangeEnergyValue(vacuumConsumption, false);
    }

    private void OnDisable()
    {
        inputReader.AnyTriggerHeld -= () => ChangeEnergyValue(vacuumConsumption, false);
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        uiBattery.UpdateEnergyUI();
    }

    private void Update()
    {
        if (inputReader.RightTriggerIsPressed || inputReader.LeftTriggerIsPressed)
        {
            actionTime += Time.deltaTime;

            if (actionTime >= actionConsumptionRate)
            {
                ChangeEnergyValue(makeNoiseConsumption, false);
                actionTime = 0f;
            }
        }
        else
        {
            actionTime = 0f;
        }
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

        actionTime = 0f;
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
}