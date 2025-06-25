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

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, cow.GetAfraidDistance(), NavMesh.AllAreas))
        {
            NavMeshPath path = new NavMeshPath();
            if (navMeshAgent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                navMeshAgent.SetPath(path);
            }
            else
            {
                Debug.LogWarning($"[{cow.name}] Failed to calculate path in AfraidState.");
            }
        }
        else
        {
            Debug.LogWarning($"[{cow.name}] Failed to find valid escape point in AfraidState.");
        }
    }

    public void UpdateState()
    {
        if (!navMeshAgent.hasPath || navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            Debug.LogWarning($"[{cow.name}] Invalid path in AfraidState. Waiting...");
            return;
        }

        animator.SetTrigger("WalkAfraid");

        var dist = Vector3.Distance(cow.transform.position, navMeshAgent.destination);

        if (!navMeshAgent.pathPending && (dist <= cow.ArrivalThreshold || navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.1f))
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