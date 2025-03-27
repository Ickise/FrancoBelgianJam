using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class UIBattery : MonoBehaviour
{
    [SerializeField, Header("References")] private TextMeshProUGUI batteryText;

    [SerializeField] private BatteryManager _batteryManager;

    private void Start()
    {
        UpdateEnergyUI();
    }

    public void UpdateEnergyUI()
    {
        batteryText.text = $"Energy: {_batteryManager.GetCurrentBattery()} / {_batteryManager.GetMaxBattery()}";
    }
}