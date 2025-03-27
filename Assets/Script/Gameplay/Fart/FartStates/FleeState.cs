using UnityEngine;
using UnityEngine.AI;

public class FleeState : IFartState
{
    private FartController fart;
    private NavMeshAgent navMeshAgent;

    public FleeState(FartController fart)
    {
        this.fart = fart;
        navMeshAgent = fart.NavMeshAgent;
    }

    public void EnterState()
    {
        // Animation de fuite
    }

    public void UpdateState()
    {
        Transform vacuum = fart.GetVacuumAttractiveArea();
        if (vacuum == null)
        {
            fart.SwitchState(new EvilState(fart));
            return;
        }

        Vector3 fleeDirection = (fart.transform.position - vacuum.position).normalized;
        Vector3 fleePosition = fart.transform.position + fleeDirection * fart.GetFleeSpeed();

        navMeshAgent.SetDestination(fleePosition);

        if (Vector3.Distance(fart.transform.position, vacuum.position) > fart.GetDistanceToBeEvil())
        {
            fart.SwitchState(new EvilState(fart));
        }
    }

    public void ExitState()
    {
    }
}