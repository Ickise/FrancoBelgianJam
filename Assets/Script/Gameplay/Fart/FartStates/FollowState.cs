using UnityEngine;
using System.Collections;

public class FollowState : IFartState
{
    private FartController fart;
    private float stateTimer;

    public FollowState(FartController fart)
    {
        this.fart = fart;
    }

    public void EnterState()
    {
        stateTimer = fart.GetTimeFollowCow();
        fart.StartCoroutine(FollowCow());
    }

    public void UpdateState()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            fart.SwitchState(new EvilState(fart));
        }
    }

    public void ExitState()
    {
        fart.StopAllCoroutines();
    }

    private IEnumerator FollowCow()
    {
        Vector3 cowPos = fart.GetCowPosition();
        while (stateTimer > 0)
        {
            fart.transform.position =
                Vector3.MoveTowards(fart.transform.position, cowPos, fart.GetSpeedFollowCow() * Time.deltaTime);
            yield return null;
        }
    }
}