using UnityEngine;
using TMPro;

public class UIBattery : MonoBehaviour
{
   [SerializeField, Header("References")] private TextMeshProUGUI energyText;
   
   private BatteryManager _batteryManager;

   private void Start()
   {
      _batteryManager = BatteryManager.instance;
      UpdateEnergyUI();
   }
   
   public void UpdateEnergyUI()
   {
      energyText.text = $"Energy: {_batteryManager.GetCurrentBattery()} / {_batteryManager.GetMaxBattery()}";
   }
}
