using TMPro;
using UnityEngine;

public class VictoryTankDetection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI victoryTankText;

    [SerializeField] private int currentGas;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.instance.GasManagerRef.GetGasStock() >= 0)
        {
            currentGas += GameManager.instance.GasManagerRef.GetGasStock();
            GameManager.instance.currentGasStock = currentGas;
            GameManager.instance.GasManagerRef.ChangeGasStockValue(currentGas, false);
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

        if (!GameManager.instance.HasPlayerWon()) return;
        
        UIManager.instance.ShowEndGameCanvas();
    }
}