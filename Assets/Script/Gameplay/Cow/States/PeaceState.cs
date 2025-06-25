using UnityEngine;
using UnityEngine.AI;

public class PeaceState : ICowState
{
    private CowController cow;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public void EnterState(CowController cow)
    {
        this.cow = cow;
        navMeshAgent = cow.NavMeshAgent;
        navMeshAgent.speed = cow.GetSpeedPeace();
        animator = cow.Animator;

        EnsureOnNavMesh();
        ResetState();
        SetRandomDestination();
    }

    public void EnterState(CowController cow, Vector3 dangerSource) {}

    public void UpdateState()
    {
        if (!navMeshAgent.isOnNavMesh || !navMeshAgent.hasPath || navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            Debug.LogWarning($"[{cow.name}] Invalid path in PeaceState. Recalculating...");
            SetRandomDestination();
            return;
        }

        animator.SetTrigger("Walk");

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            SetRandomDestination();
            Debug.Log("lets go elle bouge");
        }
    }

    public void ExitState()
    {
        ResetState();
    }

    private void ResetState()
    {
        navMeshAgent.ResetPath();
    }

    private void SetRandomDestination()
    {
        for (int attempts = 0; attempts < 10; attempts++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * Random.Range(cow.GetMinDistance(), cow.GetMaxDistance());
            randomDirection += cow.transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, cow.GetMaxDistance(), NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                if (navMeshAgent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    navMeshAgent.SetPath(path);
                    return;
                }
            }
        }

        Debug.LogWarning($"{cow.name} failed to find a valid destination after 10 attempts.");
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