using UnityEngine;
using UnityEngine.AI;

public class EvilState : IFartState
{
    private FartController fart;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private float time;
    private float timeToDisappear;

    public EvilState(FartController fart)
    {
        this.fart = fart;
        navMeshAgent = fart.NavMeshAgent;
        animator = fart.Animator;
    }

    public void EnterState()
    {
        timeToDisappear = fart.GetTimeToDisappear();

        EnsureOnNavMesh();
        ResetState();
        SetRandomDestination();
    }
    
    private void ResetState()
    {
        navMeshAgent.ResetPath();
    }

    public void UpdateState()
    {
        time += Time.deltaTime;
        animator.SetTrigger("Idle");
        
        if (!navMeshAgent.isOnNavMesh || !navMeshAgent.hasPath ||
            navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            SetRandomDestination();
        }

        if (time >= timeToDisappear)
        {
            AudioManager.instance.PlaySFX("FartDisappearance");
            Object.Destroy(fart.gameObject);
        }
    }

    public void ExitState()
    {
        time = 0f;
        ResetState();
    }

    private void EnsureOnNavMesh()
    {
        if (navMeshAgent.isOnNavMesh) return;
        
        if (NavMesh.SamplePosition(fart.transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            navMeshAgent.Warp(hit.position);
        }
    }

    private void SetRandomDestination()
    {
        Vector3 startPos = fart.transform.position;
        Vector3 playerDir = fart.GetPlayerDirection();

        for (var attempts = 0; attempts < 10; attempts++)
        {
            var dir = Quaternion.Euler(0, Random.Range(-fart.GetRangeAngle(), fart.GetRangeAngle()), 0) * -playerDir;
            var candidate = startPos + dir * Random.Range(3f, 10f);

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, fart.GetMaxDistance(), NavMesh.AllAreas))
            {
                var path = new NavMeshPath();
                
                if (navMeshAgent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    navMeshAgent.speed = fart.GetEvilSpeed();
                    navMeshAgent.SetPath(path);
                    return;
                }
            }
        }
    }
}
