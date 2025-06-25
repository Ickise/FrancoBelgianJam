using UnityEngine;
using UnityEngine.AI;

public class AfraidState : ICowState
{
    private CowController cow;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public void EnterState(CowController cow) {}

    public void EnterState(CowController cow, Vector3 dangerSource)
    {
        this.cow = cow;
        navMeshAgent = cow.NavMeshAgent;
        animator = cow.Animator;

        EnsureOnNavMesh();
        navMeshAgent.speed = cow.GetSpeedAfraid();

        Vector3 target = cow.GetEscapePosition(dangerSource, cow.GetAfraidDistance());
        navMeshAgent.SetDestination(target);
    }

    public void UpdateState()
    {
        if (!navMeshAgent.hasPath || navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            Debug.LogWarning($"[{cow.name}] Invalid path in AfraidState. Recalculating...");
            return;
        }

        animator.SetTrigger("WalkAfraid");

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            cow.SwitchState(new PeaceState());
        }
    }

    public void ExitState()
    {
        animator.SetTrigger("Walk");
        cow.ResetScaredState();
    }

    private void EnsureOnNavMesh()
    {
        if (!navMeshAgent.isOnNavMesh)
        {
            if (NavMesh.SamplePosition(cow.transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                navMeshAgent.Warp(hit.position);
            }
        }
    }
}