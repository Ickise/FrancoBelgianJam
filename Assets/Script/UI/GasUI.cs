using UnityEngine;
using TMPro;

public class GasUI : MonoBehaviour
{
    [SerializeField, Header("References")] private TextMeshProUGUI gasText;
   
    public void UpdateGasUI()
    {
        gasText.text = $"Gas stock: {GameManager.instance.GasManagerRef.GetGasStock()}";
    }
}
