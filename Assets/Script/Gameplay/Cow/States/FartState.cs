using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class FartState : ICowState
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

        if (cow.HasBeenFarted()) return;

        cow.SetFarting(true);
        cow.SetFarted(true);
        cow.SpawnFart();

        escapeDirection = -(dangerSource - cow.transform.position).normalized;
        escapeDirection.y = 0;

        Vector3 targetOffset = escapeDirection * cow.AnticipationLevel;
        Vector3 targetPosition = cow.transform.position + escapeDirection * cow.GetFartDistance() + targetOffset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, cow.GetFartDistance(), NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
    }

    public void UpdateState()
    {
        animator.SetTrigger("RunFart");

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            cow.SetFarting(false);
            cow.StartCoroutine(RecoverBeforePeace());
        }
    }

    public void ExitState()
    {
        animator.SetTrigger("Walk");
        cow.SetFarting(false);
        cow.ResetFartState();
        cow.StopAllCoroutines();
    }

    private IEnumerator RecoverBeforePeace()
    {
        yield return new WaitForSeconds(Random.Range(cow.GetMinTimeRecoverFromFart(), cow.GetMaxTimeRecoverFromFart()));
        cow.SwitchState(new PeaceState());
    }
}