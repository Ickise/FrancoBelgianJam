using UnityEngine;
using UnityEngine.AI;

public class AfraidState : ICowState
{
    private CowController cow;
    private NavMeshAgent navMeshAgent;
    private Vector3 escapeDirection;
    private Animator animator;

    public void EnterState(CowController cow)
    {
    }

    public void EnterState(CowController cow, Vector3 dangerSource)
    {
        this.cow = cow;
        navMeshAgent = cow.NavMeshAgent;
        animator = cow.Animator;
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
}