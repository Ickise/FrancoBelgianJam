using UnityEngine;

public class PeaceState : ICowState
{
    private CowController cow;
    private Vector3 targetPosition;

    public void EnterState(CowController cow)
    {
        this.cow = cow;
        SetRandomDestination();
    }

    public void EnterState(CowController cow, Vector3 dangerSource)
    {
        
    }

    public void UpdateState()
    {
        if (Vector3.Distance(cow.transform.position, targetPosition) > 0.1f)
        {
            cow.transform.position = Vector3.MoveTowards(cow.transform.position, targetPosition,
                cow.GetSpeedPeace() * Time.deltaTime);
        }
        else
        {
            SetRandomDestination();
        }
    }

    public void ExitState()
    {
    }

    private void SetRandomDestination()
    {
        float randomTime = Random.Range(cow.GetMinTimeStatic(), cow.GetMaxTimeStatic());
        float randomDistance = Random.Range(cow.GetMinDistance(), cow.GetMaxDistance());
        Vector3 randomDirection = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward;
        targetPosition = cow.transform.position + randomDirection * randomDistance;
    }
}