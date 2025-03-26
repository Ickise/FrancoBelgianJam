using UnityEngine;

public class GasManager : MonoBehaviour
{
    public static GasManager instance;

    [SerializeField, Header("References")] private UIGas uiGas;

    [SerializeField, Header("Settings")] private float maxTank = 100f;

    [SerializeField] private float maxTankOverfillRate = 1.1f;
    
    private float maxTankOverfill;
    
    private GameManager gameManager;

    private float currentGasStock = 0;

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
        
        uiGas.UpdateGasUI();
    }
    
    public bool IsGasStockOverFilled()
    {
        return currentGasStock >= maxTank;
    }
}