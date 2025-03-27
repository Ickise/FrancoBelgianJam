using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [SerializeField] private List<float> currentUpgradePrice;
    [SerializeField] private int currentPriceIndex;

    [SerializeField] private float scorePenalty = 0.2f;
    [SerializeField] private List<FacilityDetection> lFacilities;

    [SerializeField] private List<TextMeshProUGUI> upgradeTexts;
    [SerializeField] private List<UpgradesObjects> lPoolUpgrades;
    private List<UpgradesObjects> currentPool;
    [SerializeField] private List<PlayerUpgrades> lPlayerUpgrades;
    [SerializeField] private UpgradeList uList;
    [SerializeField] private PlayerMovement pMov;
    [SerializeField] private GameObject uiMenuUpgrade;
    [SerializeField] private UpgradesObjects emptyUpgrade;
     
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

        currentPriceIndex = 0;
        currentPool = new List<UpgradesObjects>();
    }

    void RisePrice()
    {
        if (currentPriceIndex + 1 >= currentUpgradePrice.Count) return;
        
        Debug.Log("Rise price");
        currentPriceIndex++;
        
        foreach (var facility in lFacilities)
        {
            facility.SetGasQuantity(GetPrice());
        }
    }

    public float GetPrice()
    {
        return currentUpgradePrice[currentPriceIndex];
    }

    public float GetPenalty()
    {
        return scorePenalty;
    }

    public void ChangeMenuUpgradeState(bool state)
    {
        if (state)
        {
            ChooseRangeRandomUpgrade();
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        uiMenuUpgrade.SetActive(state);
        
    }

    private void ChooseRangeRandomUpgrade()
    {
        currentPool.Clear();
        var newList = new List<UpgradesObjects>();
        foreach (var upgrades in lPoolUpgrades)
        {
            newList.Add(upgrades);
        }

        for (int i = 0; i < 3; i++)
        {
            if (newList.Count == 0)
            {
                currentPool.Add(emptyUpgrade);
            }
            else
            {
                var index = Random.Range(0, newList.Count);
                currentPool.Add(newList[index]); 
                newList.RemoveAt(index);
            }
        }

        for (int i = 0; i < currentPool.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                upgradeTexts[i*3 + j].text = currentPool[i].upgradeTexts[j];
            }
        }
    }

    public void ChooseUpgrade(int position)
    {
        var upgrade = currentPool[position];
        foreach (var currentUpgrade in lPlayerUpgrades)
        {
            if (currentUpgrade.type == upgrade.upgradeType)
            {
                currentUpgrade.index++;
                break;
            }
        }
        SetUpgrade(upgrade.upgradeType);
    }

    void SetUpgrade(EnumUpgradeType upgradeType)
    {
        if (upgradeType == EnumUpgradeType.None)
        {
            return;
        }
        
        if (lPlayerUpgrades[(int)upgradeType].index > 3)
        {
            Debug.Log("non");
            return;
        }
        
        RisePrice();
        ChangeMenuUpgradeState(false);
        switch (upgradeType)
        {
            case EnumUpgradeType.CharacterSpeed:
                pMov.ChangeSpeedMultiplier(uList.characterMultiplierSpeeds[lPlayerUpgrades[0].index]);
                break;

            case EnumUpgradeType.BatteryCapacity:
                BatteryManager.instance.ChangeBatteryCapacities(uList.batteryCapacities[lPlayerUpgrades[1].index], 
                    uList.batteryOverchargeCapacities[lPlayerUpgrades[1].index]);
                break;
            
            case EnumUpgradeType.GasTankCapacity:
                GasManager.instance.ChangeGasTankCapacities(uList.gasTankCapacities[lPlayerUpgrades[2].index],
                    uList.gasTankOverloadCapacities[lPlayerUpgrades[2].index]);
                break;
            
            case EnumUpgradeType.GasToBatteryConversion:
                BatteryManager.instance.ChangeConversion(uList.gasToBatteryConversions[lPlayerUpgrades[3].index]);
                break;
            
            case EnumUpgradeType.VacuumArea:
                var dist = uList.vacuumAreaDistances[lPlayerUpgrades[4].index];
                var smallAngle = uList.vacuumAreaSmallAngles[lPlayerUpgrades[4].index];
                var bigAngle = uList.vacuumAreaBigAngles[lPlayerUpgrades[4].index];
                break;

            case EnumUpgradeType.OverloadSpeedPenalty:
                pMov.ChangeOverfillSpeed(uList.overloadSpeedPenalties[lPlayerUpgrades[5].index]);
                break;
        }

        if (lPlayerUpgrades[(int)upgradeType].index == 3)
        {
            foreach (var upgrade in lPoolUpgrades)
            {
                if (upgrade.upgradeType == upgradeType)
                {
                    lPoolUpgrades.Remove(upgrade);
                    break;
                }
            }
        }
    }

    private void Start()
    {
        //GasManager.instance.ChangeGasStockValue(1000, true);
    }
}

[Serializable]
public class PlayerUpgrades
{
    public EnumUpgradeType type;
    public int index;
}