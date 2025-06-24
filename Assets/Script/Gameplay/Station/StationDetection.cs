using System;
using TMPro;
using UnityEngine;

public class StationDetection : MonoBehaviour
{
    [SerializeField] private float gasQuantity = 10;

    [SerializeField] private TextMeshProUGUI gasText;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        //Debug.Log("try recharge");

        if (GameManager.instance.GasManagerRef.GetGasStock() >= gasQuantity)
        {
            GameManager.instance?.BatteryManagerRef.RechargeBattery(gasQuantity);
            GameManager.instance?.GasManagerRef.ChangeGasStockValue(gasQuantity, false);
        }
        else
        {
            //Debug.Log("No recharge");
        }
    }

    private void Start()
    {
        gasText.text = $"Need {gasQuantity} gas to recharge battery!";
    }
}
