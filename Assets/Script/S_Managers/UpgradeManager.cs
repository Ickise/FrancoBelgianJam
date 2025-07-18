using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Buttons")] [SerializeField]
    private Button leftUpgradeButton;

    [SerializeField] private Button middleUpgradeButton;
    [SerializeField] private Button rightUpgradeButton;

    [Header("Upgrade Settings")] [SerializeField]
    private List<int> currentUpgradePrice;

    [SerializeField] private float scorePenalty = 0.2f;
    [SerializeField] private List<FacilityDetection> lFacilities;
    [SerializeField] private List<TextMeshProUGUI> upgradeTexts;
    [SerializeField] private List<UpgradesObjects> lPoolUpgrades;
    [SerializeField] private List<PlayerUpgrades> lPlayerUpgrades;
    [SerializeField] private UpgradeList uList;
    [SerializeField] private PlayerMovement pMov;
    [SerializeField] private UpgradesObjects emptyUpgrade;
    [SerializeField] private EventSystem eventSystem;

    private List<UpgradesObjects> currentPool;
    private int currentPriceIndex;

    private void Awake()
    {
        currentPriceIndex = 0;
        currentPool = new List<UpgradesObjects>();
    }

    public void UpdateUpgradeMenu()
    {
        if (AreAllUpgradesPurchased()) return;

        UIManager.instance.ShowUpgradeCanvas();
        SetupUpgradeMenu();
    }

    private void SetupUpgradeMenu()
    {
        ChooseRangeRandomUpgrade();
        SetupButtons();
        eventSystem.SetSelectedGameObject(middleUpgradeButton.gameObject);
    }

    private void ChooseRangeRandomUpgrade()
    {
        currentPool.Clear();
        var availableUpgrades = GetAvailableUpgrades();

        for (var i = 0; i < 3; i++)
        {
            currentPool.Add(availableUpgrades.Count > 0 ? GetRandomUpgrade(availableUpgrades) : emptyUpgrade);
        }

        UpdateUpgradeTexts();
        UpdateButtonVisibility();
    }

    private List<UpgradesObjects> GetAvailableUpgrades()
    {
        var availableUpgrades = new List<UpgradesObjects>();

        foreach (var upgrade in lPoolUpgrades)
        {
            var playerUpgrade = lPlayerUpgrades.Find(u => u.type == upgrade.upgradeType);
            if (playerUpgrade != null && playerUpgrade.index < 3)
            {
                availableUpgrades.Add(upgrade);
            }
        }

        return availableUpgrades;
    }

    private UpgradesObjects GetRandomUpgrade(List<UpgradesObjects> upgrades)
    {
        var index = Random.Range(0, upgrades.Count);
        var selectedUpgrade = upgrades[index];
        upgrades.RemoveAt(index);
        return selectedUpgrade;
    }

    private void UpdateUpgradeTexts()
    {
        for (var i = 0; i < currentPool.Count; i++)
        {
            var upgrade = currentPool[i];
            upgradeTexts[i * 3].text = upgrade.upgradeTexts[0];
            upgradeTexts[i * 3 + 1].text = upgrade.upgradeTexts[1];

            var playerUpgrade = lPlayerUpgrades.Find(u => u.type == upgrade.upgradeType);
            upgradeTexts[i * 3 + 2].text = playerUpgrade != null ? $"Level {playerUpgrade.index + 1}" : "Level 1";
        }
    }

    private void UpdateButtonVisibility()
    {
        leftUpgradeButton.gameObject.SetActive(IsUpgradeAvailable(0));
        middleUpgradeButton.gameObject.SetActive(IsUpgradeAvailable(1));
        rightUpgradeButton.gameObject.SetActive(IsUpgradeAvailable(2));
    }

    private bool IsUpgradeAvailable(int index)
    {
        return index < currentPool.Count && currentPool[index].upgradeType != EnumUpgradeType.None;
    }

    private void SetupButtons()
    {
        leftUpgradeButton.onClick.RemoveAllListeners();
        middleUpgradeButton.onClick.RemoveAllListeners();
        rightUpgradeButton.onClick.RemoveAllListeners();

        leftUpgradeButton.onClick.AddListener(() => ApplyUpgrade(0));
        middleUpgradeButton.onClick.AddListener(() => ApplyUpgrade(1));
        rightUpgradeButton.onClick.AddListener(() => ApplyUpgrade(2));
    }

    private void ApplyUpgrade(int position)
    {
        var upgrade = currentPool[position];
        IncrementUpgradeLevel(upgrade.upgradeType);
        SetUpgrade(upgrade.upgradeType);
        UIManager.instance.ShowUpgradeCanvas();
    }

    private void IncrementUpgradeLevel(EnumUpgradeType upgradeType)
    {
        var playerUpgrade = lPlayerUpgrades.Find(u => u.type == upgradeType);
        if (playerUpgrade != null)
        {
            playerUpgrade.index++;
        }
    }

    private void SetUpgrade(EnumUpgradeType upgradeType)
    {
        if (upgradeType == EnumUpgradeType.None) return;

        var playerUpgrade = lPlayerUpgrades[(int)upgradeType];
        if (playerUpgrade.index > 3) return;

        RisePrice();
        ApplyUpgradeEffect(upgradeType, playerUpgrade.index);

        if (playerUpgrade.index == 3)
        {
            RemoveUpgradeFromPool(upgradeType);
        }
    }

    private void ApplyUpgradeEffect(EnumUpgradeType upgradeType, int level)
    {
        switch (upgradeType)
        {
            case EnumUpgradeType.CharacterSpeed:
                pMov.ChangeSpeedMultiplier(uList.characterMultiplierSpeeds[level]);
                break;
            case EnumUpgradeType.BatteryCapacity:
                GameManager.instance?.BatteryManagerRef.ChangeBatteryCapacities(
                    uList.batteryCapacities[level], uList.batteryOverchargeCapacities[level]);
                break;
            case EnumUpgradeType.GasTankCapacity:
                GameManager.instance.GasManagerRef.ChangeGasTankCapacities(
                    uList.gasTankCapacities[level], uList.gasTankOverloadCapacities[level]);
                break;
            case EnumUpgradeType.GasToBatteryConversion:
                GameManager.instance?.BatteryManagerRef.ChangeConversion(uList.gasToBatteryConversions[level]);
                break;
            case EnumUpgradeType.VacuumArea:
                break;
            case EnumUpgradeType.OverloadSpeedPenalty:
                pMov.ChangeOverfillSpeed(uList.overloadSpeedPenalties[level]);
                break;
        }
    }

    private void RemoveUpgradeFromPool(EnumUpgradeType upgradeType)
    {
        lPoolUpgrades.RemoveAll(upgrade => upgrade.upgradeType == upgradeType);
    }

    private void RisePrice()
    {
        if (currentPriceIndex + 1 >= currentUpgradePrice.Count) return;

        currentPriceIndex++;
        foreach (var facility in lFacilities)
        {
            facility.SetGasQuantity(GetPrice());
        }
    }

    public bool AreAllUpgradesPurchased()
    {
        return lPlayerUpgrades.TrueForAll(upgrade => upgrade.index >= 3);
    }

    public void NotifyFacilitiesAllUpgradesObtained()
    {
        if (!AreAllUpgradesPurchased()) return;
        
        foreach (var facility in lFacilities)
        {
            facility.SetUpgradesObtainedText();
        }
    }

    public int GetPrice() => currentUpgradePrice[currentPriceIndex];
    public float GetPenalty() => scorePenalty;
}

[Serializable]
public class PlayerUpgrades
{
    public EnumUpgradeType type;
    public int index;
}