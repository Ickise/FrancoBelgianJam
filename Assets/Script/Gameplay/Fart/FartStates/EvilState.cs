using UnityEngine;
using UnityEngine.AI;

public class EvilState : IFartState
{
    private FartController fart;
    private NavMeshAgent navMeshAgent;

    private float time;
    private float timeToDisappear;

    private Vector3 newDirection;

    public EvilState(FartController fart)
    {
        this.fart = fart;
        navMeshAgent = fart.NavMeshAgent;
    }

    public void EnterState()
    {
        timeToDisappear = fart.GetTimeToDisappear();
        Vector3 playerDir = fart.GetPlayerDirection();

        newDirection = Quaternion.Euler(0, Random.Range(-fart.GetRangeAngle(), fart.GetRangeAngle()), 0) * -playerDir;

        navMeshAgent.speed = fart.GetEvilSpeed();
        navMeshAgent.SetDestination(fart.transform.position + newDirection * 10f);
    }

    public void UpdateState()
    {
        time += Time.deltaTime;

        if (time >= timeToDisappear)
        {
            Object.Destroy(fart.gameObject);
        }
    }

    public void ExitState()
    {
        time = 0f;
        newDirection = Vector3.zero;
    }
}