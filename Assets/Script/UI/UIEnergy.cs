using UnityEngine;
using TMPro;

public class UIEnergy : MonoBehaviour
{
   [SerializeField, Header("References")] private TextMeshProUGUI energyText;
   
   private EnergyManager energyManager;

   private void Start()
   {
      energyManager = EnergyManager.instance;
      UpdateEnergyUI();
   }
   
   public void UpdateEnergyUI()
   {
      energyText.text = $"Energy: {energyManager.GetCurrentEnergy()} / {energyManager.GetMaxEnergy()}";
   }
}
