using UnityEngine;
using UnityEngine.AI;

public class FollowState : IFartState
{
    private FartController fart;
    private NavMeshAgent navMeshAgent;
    private float stateTimer;
    private CowController cowController;

    public FollowState(FartController fart)
    {
        this.fart = fart;
        navMeshAgent = fart.NavMeshAgent;
        cowController = this.fart.GetComponentInParent<CowController>();
    }

    public void EnterState()
    {
        stateTimer = fart.GetTimeFollowCow();
        navMeshAgent.speed = fart.GetSpeedFollowCow();
    }

    public void UpdateState()
    {
        FollowCow();

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            fart.SwitchState(new EvilState(fart));
        }
    }

    public void ExitState()
    {
    }

    private void FollowCow()
    {
        Vector3 cowPos = cowController.transform.position;
        navMeshAgent.SetDestination(cowPos);
    }
}