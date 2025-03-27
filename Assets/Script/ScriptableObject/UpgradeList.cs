using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeList", menuName = "Scriptable Objects/UpgradeList")]
public class UpgradeList : ScriptableObject
{
    public float[] characterMultiplierSpeeds;

    public float[] batteryCapacities;
    public float[] batteryOverchargeCapacities;
    
    public float[] gasTankCapacities;
    public float[] gasTankOverloadCapacities;
    
    public float[] gasToBatteryConversions;
    
    public float[] vacuumAreaDistances;
    
    public float[] vacuumAreaSmallAngles;
    public float[] vacuumAreaBigAngles;

    public float[] overloadSpeedPenalties;
}
