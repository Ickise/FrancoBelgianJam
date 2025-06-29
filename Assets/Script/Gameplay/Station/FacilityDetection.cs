using TMPro;
using UnityEngine;

public class FacilityDetection : MonoBehaviour
{
    [SerializeField] private int scoreToGain = 100;
    [SerializeField] private float scoreMultiplier = 2;

    [SerializeField] private TextMeshProUGUI upgradeText;

    private float gasQuantity = 80;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!(GameManager.instance.GasManagerRef.GetGasStock() >= gasQuantity)) return;

        ScoreManager.instance.ChangeScoreValue((int)(scoreToGain * scoreMultiplier), true);
        scoreMultiplier -= scoreMultiplier * UpgradeManager.instance.GetPenalty();
        scoreMultiplier = Mathf.Clamp(scoreMultiplier, 0.2f, 2f);

        GameManager.instance.GasManagerRef.ChangeGasStockValue(gasQuantity, false);

        UpgradeManager.instance.UpdateUpgradeMenu();
    }

    private void Start()
    {
        gasQuantity = UpgradeManager.instance.GetPrice();
        upgradeText.text = $"Need {gasQuantity} gas to upgrade!";
    }

    public void SetGasQuantity(float newQuantity)
    {
        gasQuantity = newQuantity;
        upgradeText.text = $"Need {gasQuantity} gas to upgrade!";
    }
}