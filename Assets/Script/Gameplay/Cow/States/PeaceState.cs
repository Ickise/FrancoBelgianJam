using UnityEngine;
using UnityEngine.AI;

public class PeaceState : ICowState
{
    private CowController cow;
    private NavMeshAgent navMeshAgent;

    public void EnterState(CowController cow)
    {
        this.cow = cow;
        navMeshAgent = cow.NavMeshAgent;
        navMeshAgent.speed = cow.GetSpeedPeace();
        SetRandomDestination();
    }

    public void EnterState(CowController cow, Vector3 dangerSource)
    {
    }

    public void UpdateState()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
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
        
        if (NavMesh.SamplePosition(randomDirection, out hit, cow.GetMaxDistance(), NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
    }
}