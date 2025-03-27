using UnityEngine;
using TMPro;

public class UIGas : MonoBehaviour
{
    [SerializeField, Header("References")] private TextMeshProUGUI gasText;
   
    [SerializeField] private GasManager gasManager;
    private void Start()
    {
        UpdateGasUI();
    }
   
    public void UpdateGasUI()
    {
        gasText.text = $"Gas stock: {gasManager.GetGasStock()}";
    }
}
