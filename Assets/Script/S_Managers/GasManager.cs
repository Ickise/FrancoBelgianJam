using UnityEngine;

public class GasManager : MonoBehaviour
{
    [SerializeField, Header("Settings")] private float maxTank = 100f;

    [SerializeField] private float maxTankOverfillRate = 1.3f;

    private float maxTankOverfill;

    private float currentGasStock = 0;

    private void Awake()
    {
        maxTankOverfill = maxTank * maxTankOverfillRate;
    }

    public float GetGasStock()
    {
        return currentGasStock;
    }

    public void ChangeGasStockValue(float amount, bool isIncreasing)
    {
        currentGasStock = isIncreasing ? currentGasStock + amount : currentGasStock - amount;

        currentGasStock = Mathf.Clamp(currentGasStock, 0, maxTankOverfill);

        UIManager.instance.GasUI.UpdateGasUI();
    }

    public bool IsGasStockOverFilled()
    {
        return currentGasStock > maxTank;
    }

    public void ChangeGasTankCapacities(float baseCapa, float overCapa)
    {
        maxTank = baseCapa;
        maxTankOverfill = overCapa;
        UIManager.instance.GasUI.UpdateGasUI();
    }
    
    [ContextMenu("Gain Gas")]
    public void GainGas()
    {
        ChangeGasStockValue(1000f, true);
        Debug.Log("Gained 1000 gas.");
    }
}