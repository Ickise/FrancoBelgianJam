using UnityEngine;

public class AfraidState : ICowState
{
    private CowController cow;
    private Vector3 escapeDirection;
    private float remainingDistance;

    public void EnterState(CowController cow)
    {
    }

    public void EnterState(CowController cow, Vector3 dangerSource)
    {
        this.cow = cow;
        escapeDirection = (cow.transform.position - dangerSource).normalized;
        remainingDistance = cow.GetAfraidDistance();
    }

    public void UpdateState()
    {
        if (remainingDistance > 0)
        {
            float moveStep = cow.GetSpeedAfraid() * Time.deltaTime;
            cow.transform.position += escapeDirection * moveStep;
            remainingDistance -= moveStep;
        }
        else
        {
            cow.SwitchState(new PeaceState());
        }
    }

    public void ExitState()
    {
        cow.ResetScaredState();
    }
}