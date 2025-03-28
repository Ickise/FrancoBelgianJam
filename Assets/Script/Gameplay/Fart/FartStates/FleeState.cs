using UnityEngine;
using UnityEngine.AI;

public class FleeState : IFartState
{
    private FartController fart;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public FleeState(FartController fart)
    {
        this.fart = fart;
        navMeshAgent = fart.NavMeshAgent;
        animator = fart.Animator;
    }

    public void EnterState()
    {
    }

    public void UpdateState()
    {
        Transform vacuum = fart.GetVacuumAttractiveArea();
        animator.SetTrigger("Run");

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