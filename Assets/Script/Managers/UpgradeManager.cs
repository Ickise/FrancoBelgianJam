using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [SerializeField] private List<float> currentUpgradePrice;
    [SerializeField] private int currentPriceIndex;

    [SerializeField] private float scorePenalty = 0.2f;
    [SerializeField] private List<FacilityDetection> lFacilities;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentPriceIndex = 0;
        
    }

    public void RisePrice()
    {
        if (currentPriceIndex + 1 >= currentUpgradePrice.Count) return;
        
        Debug.Log("Rise price");
        currentPriceIndex++;
        
        foreach (var facility in lFacilities)
        {
            facility.SetGasQuantity(GetPrice());
        }
    }

    public float GetPrice()
    {
        return currentUpgradePrice[currentPriceIndex];
    }

    public float GetPenalty()
    {
        return scorePenalty;
    }
}
