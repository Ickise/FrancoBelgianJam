using UnityEngine;
using UnityEngine.AI;

public class AfraidState : ICowState
{
    private CowController cow;
    private NavMeshAgent navMeshAgent;
    private Vector3 escapeDirection;

    public void EnterState(CowController cow)
    {
    }

    public void EnterState(CowController cow, Vector3 dangerSource)
    {
        this.cow = cow;
        navMeshAgent = cow.NavMeshAgent;

        escapeDirection = (cow.transform.position - dangerSource).normalized;
        escapeDirection.y = 0;

        navMeshAgent.speed = cow.GetSpeedAfraid();

        Vector3 targetOffset = escapeDirection * cow.AnticipationLevel;
        Vector3 targetPosition = cow.transform.position + escapeDirection * cow.GetAfraidDistance() + targetOffset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, cow.GetAfraidDistance(), NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
    }

    public void UpdateState()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            cow.SwitchState(new PeaceState());
        }
    }

    public void ExitState()
    {
        cow.ResetScaredState();
    }
}