using UnityEngine;

public interface ICowState
{
    void EnterState(CowController cow);
    void EnterState(CowController cow, Vector3 dangerSource);
    void UpdateState();
    void ExitState();
}