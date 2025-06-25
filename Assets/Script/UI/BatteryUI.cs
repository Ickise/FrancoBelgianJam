using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BatteryUI : MonoBehaviour
{
    [SerializeField, Header("UI References")]
    private TextMeshProUGUI batteryText;

    [SerializeField] private TextMeshProUGUI overchargeText;

    [SerializeField] private GameObject overchargeGameObject;

    [SerializeField] private List<GameObject> batteryImages;

    public void InitializeBatteryUI()
    {
        UpdateEnergyUI();

        overchargeGameObject.SetActive(false);
        overchargeText.gameObject.SetActive(true);
    }

    public void UpdateEnergyUI()
    {
        batteryText.text =
            $"Energy: {GameManager.instance.BatteryManagerRef.GetCurrentBattery()} / {GameManager.instance.BatteryManagerRef.GetMaxBattery()}";
        UpdateBatteryDisplay(GameManager.instance.BatteryManagerRef.GetCurrentBattery());
    }

    private void UpdateBatteryDisplay(float currentBattery)
    {
        var batteryValue = GameManager.instance.BatteryManagerRef.GetMaxBattery() / batteryImages.Count;

        var activeBatteryCount = Mathf.FloorToInt(currentBattery / batteryValue);

        for (var i = 0; i < batteryImages.Count; i++)
        {
            if (batteryImages[i] != null)
            {
                batteryImages[i].SetActive(i < activeBatteryCount);

            }
            else
            {
                Debug.LogWarning($"Battery image at index {i} is missing or destroyed.");
            }
        }
    }

    public void EnableOverchargeUI(bool enable)
    {
        overchargeGameObject.SetActive(enable);
    }
}