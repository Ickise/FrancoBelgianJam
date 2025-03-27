using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesObject", menuName = "Scriptable Objects/UpgradesObjects")]
public class UpgradesObjects : ScriptableObject
{
    public EnumUpgradeType upgradeType;
    public List<string> upgradeTexts;

}

public enum EnumUpgradeType
{
    CharacterSpeed = 0,
    BatteryCapacity = 1,
    GasTankCapacity = 2,
    GasToBatteryConversion = 3,
    VacuumAreaDistance = 4,
    VacuumAreaAngle = 5,
    OverloadSpeedPenalty = 6,
}
