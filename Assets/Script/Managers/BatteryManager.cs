using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    public static BatteryManager instance;

    [SerializeField, Header("References")] private UIBattery uiBattery;
    [SerializeField] private GameObject overchargedText; 
    [SerializeField] private GameObject overchargedEffect; 
    
    [SerializeField] private InputReader inputReader;

    [SerializeField, Header("Settings")] private float maxBattery = 100f;
    [SerializeField] private float vacuumConsumption = 2f;
    [SerializeField] private float makeNoiseConsumption = 2f;
    [SerializeField] private float overchargeConsumption = 1f;
    [SerializeField] private float gasIntoEnergyConversion = 2f;
    [SerializeField] private float vacuumConsumptionRate = 1f;
    [SerializeField] private float makeSoundConsumptionRate = 1f;
    [SerializeField] private float overchargeDepletionRate = 0.3f;
    [SerializeField] private float actionConsumptionRate = 1f;
    [SerializeField] private float overchargeRate = 1.5f;
    
    private float currentBattery;
    private float maxOvercharge;

    private float actionTime;
    private float overchargeTime;

    private GameManager gameManager;

    private bool isOvercharge;

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
        maxOvercharge = maxBattery * overchargeRate;
    }

    private void OnEnable()
    {
        inputReader.RightTriggerEvent += () => ChangeEnergyValue(vacuumConsumption, false);
        inputReader.LeftTriggerEvent += () => ChangeEnergyValue(makeNoiseConsumption, false);
    }

    private void OnDisable()
    {
        inputReader.RightTriggerEvent -= () => ChangeEnergyValue(vacuumConsumption, false);
        inputReader.LeftTriggerEvent -= () => ChangeEnergyValue(makeNoiseConsumption, false);
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        uiBattery.UpdateEnergyUI();
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
        
        overchargedText.SetActive(BatteryOvercharging());
        
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
        uiBattery.UpdateEnergyUI();

        if (currentBattery <= 0)
        {
            gameManager.GameOver();
        }
    }

    public void RechargeBattery(float gasAmount)
    {
        if (gasAmount <= 0) return;

        float energyGained = gasAmount * gasIntoEnergyConversion;
        ChangeEnergyValue(energyGained, true);
    }

    public bool BatteryOvercharging()
    {
        isOvercharge = currentBattery > maxBattery;
        overchargedEffect.SetActive(isOvercharge);
        return isOvercharge;
    }

    public float GetCurrentBattery()
    {
        return currentBattery;
    }

    public float GetMaxBattery()
    {
        return maxBattery;
    }

    public void ChangeBatteryCapacities(float baseCapa, float overCapa)
    {
        maxBattery = baseCapa;
        maxOvercharge = overCapa;
        uiBattery.UpdateEnergyUI();
    }
}