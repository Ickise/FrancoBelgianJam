using UnityEngine;

public class GasManager : MonoBehaviour
{
    public static GasManager instance;

    [SerializeField, Header("References")] private UIGas uiGas;
    
    private GameManager gameManager;

    private int currentGasStock = 0;

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
    }

    public int GetGasStock()
    {
        return currentGasStock;
    }

    public void ChangeGasStockValue(int amount, bool isIncreasing)
    {
        currentGasStock = isIncreasing ? currentGasStock + amount : currentGasStock - amount;
        uiGas.UpdateGasUI();
    }
}