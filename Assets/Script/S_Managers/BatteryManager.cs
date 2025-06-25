using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    [SerializeField, Header("Settings")] private float maxBattery = 100f;
    [SerializeField] private float vacuumConsumption = 2f;
    [SerializeField] private float makeNoiseConsumption = 2f;
    [SerializeField] private float overchargeConsumption = 1f;
    [SerializeField] private float gasIntoEnergyConversion = 2f;
    [SerializeField] private float vacuumConsumptionRate = 1f;
    [SerializeField] private float makeSoundConsumptionRate = 1f;
    [SerializeField] private float overchargeDepletionRate = 0.3f;
    [SerializeField] private float overchargeRate = 1.5f;

    private float currentBattery;
    private float maxOvercharge;

    private float actionTime;
    private float overchargeTime;

    private bool isOvercharge;

    private InputReader inputReader;
    private BatteryUI batteryUI;

    private void Awake()
    {
        currentBattery = maxBattery;
        maxOvercharge = maxBattery * overchargeRate;
        
        inputReader = GameManager.instance.InputReader;
        batteryUI = UIManager.instance.BatteryUI;
    }

    private void OnEnable()
    {
        inputReader.RightTriggerEvent += HandleRightTrigger;
        inputReader.LeftTriggerEvent += HandleLeftTrigger;
    }

    private void OnDisable()
    {
        inputReader.RightTriggerEvent -= HandleRightTrigger;
        inputReader.LeftTriggerEvent -= HandleLeftTrigger;
    }

    private void Update()
    {
        // I know this is a duplicate, but it's a game jam. If I have more time, I will refacto this!
        if (inputReader.RightTriggerIsPressed)
        {
            actionTime += Time.deltaTime;

            if (actionTime >= vacuumConsumptionRate)
            {
                ChangeEnergyValue(vacuumConsumption, false);
                actionTime = 0f;
            }
        }

        if (inputReader.LeftTriggerIsPressed)
        {
            actionTime += Time.deltaTime;

            if (actionTime >= makeSoundConsumptionRate)
            {
                ChangeEnergyValue(makeNoiseConsumption, false);
                actionTime = 0f;
            }
        }

        if (!inputReader.RightTriggerIsPressed && !inputReader.LeftTriggerIsPressed)
        {
            actionTime = 0f;
        }

        batteryUI.EnableOverchargeUI(BatteryOvercharging());

        if (!BatteryOvercharging()) return;

        DefleteBatteryOnOvercharging();
    }

    private void DefleteBatteryOnOvercharging()
    {
        overchargeTime += Time.deltaTime;

        if (overchargeTime <= overchargeDepletionRate) return;

        ChangeEnergyValue(overchargeConsumption, false);
        overchargeTime = 0f;
    }
    
    private void HandleRightTrigger()
    {
        ChangeEnergyValue(vacuumConsumption, false);
    }

    private void HandleLeftTrigger()
    {
        ChangeEnergyValue(makeNoiseConsumption, false);
    }

    public void ChangeEnergyValue(float amount, bool isIncreasing)
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
        batteryUI.UpdateEnergyUI();

        if (currentBattery <= 0)
        {
            UIManager.instance.ShowEndGameCanvas();
        }
    }

    public void RechargeBattery(float gasAmount)
    {
        if (gasAmount <= 0) return;

        var energyGained = gasAmount * gasIntoEnergyConversion;
        ChangeEnergyValue(energyGained, true);
        AudioManager.instance.PlaySFX("ReloadBattery");
    }

    public bool BatteryOvercharging()
    {
        isOvercharge = currentBattery > maxBattery;
        batteryUI.EnableOverchargeUI(isOvercharge);
        return isOvercharge;
    }

    public void ChangeBatteryCapacities(float baseCapa, float overCapa)
    {
        maxBattery = baseCapa;
        maxOvercharge = overCapa;
        batteryUI.UpdateEnergyUI();
    }

    public void ChangeConversion(float value)
    {
        gasIntoEnergyConversion = value;
    }
    
    public float GetCurrentBattery() => currentBattery;
    public float GetMaxBattery() => maxBattery;
}