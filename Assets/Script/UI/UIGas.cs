using UnityEngine;
using TMPro;

public class UIGas : MonoBehaviour
{
    [SerializeField, Header("References")] private TextMeshProUGUI gasText;
   
    private GasManager gasManager;

    private void Start()
    {
        gasManager = GasManager.instance;
        UpdateGasUI();
    }
   
    public void UpdateGasUI()
    {
        gasText.text = $"Gas stock: {gasManager.GetGasStock()}";
    }
}
