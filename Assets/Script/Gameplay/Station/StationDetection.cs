using System;
using UnityEngine;

public class StationDetection : MonoBehaviour
{
    [SerializeField] private float gasQuantity = 10;
     
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BatteryManager.instance.RechargeBattery(gasQuantity);
            GasManager.instance.ChangeGasStockValue(gasQuantity, false);
        }
    }
}
