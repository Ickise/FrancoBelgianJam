using TMPro;
using UnityEngine;

public class VictoryTankDetection : MonoBehaviour
{
    [SerializeField] private float gasDeposit = 50;
    [SerializeField] private TextMeshProUGUI victoryTankText;

    [SerializeField] private float currentGas;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        //Debug.Log("try deposit");

        if (GameManager.instance.GasManagerRef.GetGasStock() >= gasDeposit)
        {
            currentGas += gasDeposit;
            GameManager.instance.GasManagerRef.ChangeGasStockValue(gasDeposit, false);
        }
        else
        {
            currentGas += GameManager.instance.GasManagerRef.GetGasStock();
            GameManager.instance.GasManagerRef.ChangeGasStockValue(GameManager.instance.GasManagerRef.GetGasStock(),
                false);
        }

        Deposit();
    }

    private void Start()
    {
        Deposit();
    }

    void Deposit()
    {
        victoryTankText.text =
            $"Deposit {currentGas} / {GameManager.instance.GetGasThreshold()} Gas to supply the city and win";

        if (currentGas >= GameManager.instance.GetGasThreshold())
        {
            UIManager.instance.ShowEndGameCanvas();
        }
    }
}