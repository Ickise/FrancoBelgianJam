using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class FartState : ICowState
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

        if (cow.HasBeenFarted()) return;
        cow.SetFarted(true);

        EnsureOnNavMesh();

        Vector3 target = cow.GetEscapePosition(dangerSource, cow.GetFartDistance());
        navMeshAgent.SetDestination(target);

        cow.StartCoroutine(FartRoutine());
    }

    public void UpdateState()
    {
        animator.SetTrigger("RunFart");
    }

    public void ExitState()
    {
        animator.SetTrigger("Walk");
        cow.ResetFartState();
        cow.StopAllCoroutines();
    }

    private IEnumerator FartRoutine()
    {
        float duration = Random.Range(cow.GetMinTimeRecoverFromFart(), cow.GetMaxTimeRecoverFromFart());
        float timer = 0f;

        while (timer < duration)
        {
            cow.SpawnFart();
            float waitTime = Random.Range(0.7f, 2f);
            yield return new WaitForSeconds(waitTime);
            timer += waitTime;
        }

        cow.SwitchState(new PeaceState());
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