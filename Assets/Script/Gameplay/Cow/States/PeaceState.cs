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
        SetRandomDestination();
    }

    public void EnterState(CowController cow, Vector3 dangerSource)
    {
    }

    public void UpdateState()
    {
        animator.SetTrigger("Walk");

        if (navMeshAgent.isOnNavMesh && !navMeshAgent.pathPending &&
            navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            SetRandomDestination();
        }
    }

    public void ExitState()
    {
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * Random.Range(cow.GetMinDistance(), cow.GetMaxDistance());
        randomDirection += cow.transform.position;

        NavMeshHit hit;

        if (!navMeshAgent.isOnNavMesh)
        {
            if (NavMesh.SamplePosition(cow.transform.position, out hit, 1f, NavMesh.AllAreas))
            {
                navMeshAgent.Warp(hit.position);
            }
        }
        else
        {
            if (NavMesh.SamplePosition(randomDirection, out hit, 1f, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
            }
        }
    }
}