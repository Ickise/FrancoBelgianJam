using TMPro;
using UnityEngine;

public class VictoryTankDetection : MonoBehaviour
{
    [SerializeField] private float gasDeposit = 50;
    [SerializeField] private float currentGas;
    [SerializeField] private TextMeshProUGUI victoryTankText;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        //Debug.Log("try deposit");

        if (GasManager.instance.GetGasStock() >= gasDeposit)
        {
            currentGas += gasDeposit;
            GasManager.instance.ChangeGasStockValue(gasDeposit, false);
        }
        else
        {
            currentGas += GasManager.instance.GetGasStock();
            GasManager.instance.ChangeGasStockValue(GasManager.instance.GetGasStock(), false);
        }
        Deposit();
    }

    private void Start()
    {
        Deposit();
    }

    void Deposit()
    {
        victoryTankText.text = $"Deposit {currentGas} / {GameManager.instance.GetGasThreshold()} Gas to supply the city and win";
        GameManager.instance.Victory(currentGas);
    }
    
    
}

