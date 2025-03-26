using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager instance;
    
    [SerializeField, Header("References")] private UIEnergy uiEnergy;

    [SerializeField, Header("Settings")] private int maxEnergy = 100;
    
    private int currentEnergy;

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
        
        currentEnergy = maxEnergy;
    }

    public void ChangeEnergyValue(int amount, bool isIncreasing)
    {
        currentEnergy = isIncreasing ? currentEnergy + amount : currentEnergy - amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        uiEnergy.UpdateEnergyUI();
    }
    
    public int GetCurrentEnergy()
    {
        return currentEnergy;
    }
    
    public int GetMaxEnergy()
    {
        return maxEnergy;
    }
}