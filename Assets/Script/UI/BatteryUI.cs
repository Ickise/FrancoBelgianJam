using UnityEngine;
using TMPro;

public class BatteryUI : MonoBehaviour
{
    [SerializeField, Header("UI References")] private TextMeshProUGUI batteryText;

    [SerializeField] private GameObject[] batteryImages;

    public void UpdateEnergyUI()
    {
        batteryText.text = $"Energy: {GameManager.instance.BatteryManagerRef.GetCurrentBattery()} / {GameManager.instance.BatteryManagerRef.GetMaxBattery()}";
        UpdateBatteryDisplay(GameManager.instance.BatteryManagerRef.GetCurrentBattery());
    }

    private void UpdateBatteryDisplay(float currentBattery)
    {
        var batteryValue = 100 / batteryImages.Length;

        var activeBatteryCount = Mathf.FloorToInt(currentBattery / batteryValue);

        for (var i = 0; i < batteryImages.Length; i++)
        {
            batteryImages[i].SetActive(i < activeBatteryCount);
        }
    }
}