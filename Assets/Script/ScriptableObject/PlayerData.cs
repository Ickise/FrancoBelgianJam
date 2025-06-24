using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float speed = 3f;
    public float overchargeSpeed = 5f;
    public float overfillSpeedRate = 0.75f;
    public float moveConsumption = 1f;
    public float moveConsumptionRate = 1f;
}