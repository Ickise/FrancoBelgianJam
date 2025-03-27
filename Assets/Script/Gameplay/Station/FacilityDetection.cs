using System;
using UnityEngine;

public class FacilityDetection : MonoBehaviour
{
    private float _gasQuantity = 80;
    private int _scoreToGain = 100;
    private float _scoreMultiplier = 2;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("try upgrade");
        
        if (GasManager.instance.GetGasStock() >= _gasQuantity)
        {
            ScoreManager.instance.ChangeScoreValue((int)(_scoreToGain * _scoreMultiplier), true);
            _scoreMultiplier -= _scoreMultiplier * UpgradeManager.instance.GetPenalty();
            _scoreMultiplier = Mathf.Clamp(_scoreMultiplier, 0.2f,2f);
            
            GasManager.instance.ChangeGasStockValue(_gasQuantity, false);
            
            UpgradeManager.instance.RisePrice();
        }
        else
        {
            Debug.Log("No upgrade");
        }
    }

    private void Start()
    {
        _gasQuantity = UpgradeManager.instance.GetPrice();
    }

    public void SetGasQuantity(float newQuantity)
    {
        _gasQuantity = newQuantity;
    }
}
