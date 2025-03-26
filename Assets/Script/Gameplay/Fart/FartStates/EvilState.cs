using UnityEngine;
using System.Collections;

public class EvilState : IFartState
{
    private FartController fart;
    private float stateTimer;
    private float timeToDisappear;
    private bool followingCow = true;

    private float time;

    public EvilState(FartController fart)
    {
        this.fart = fart;
    }

    public void EnterState()
    {
        stateTimer = fart.GetTimeFollowCow();
        timeToDisappear = fart.GetTimeToDisappear();
        fart.StartCoroutine(FollowCow());
        // ANIMATION : WALK
    }

    public void UpdateState()
    {
        if (!followingCow)
        {
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0)
            {
                Object.Destroy(fart.gameObject);
            }
        }

        time += Time.deltaTime;

        if (time >= timeToDisappear)
        {
            Object.Destroy(fart.gameObject);
        }
    }

    public void ExitState()
    {
        fart.StopAllCoroutines();
        time = 0f;
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

        followingCow = false;
        // Walking animation
        yield return new WaitForSeconds(0.8f);

        Vector3 playerDir = fart.GetPlayerDirection();
        Vector3 newDirection = Quaternion.Euler(0, Random.Range(-fart.GetRangeAngle(), fart.GetRangeAngle()), 0) *
                               -playerDir;
        fart.transform.position += newDirection.normalized * fart.GetEvilSpeed() * Time.deltaTime;

        // Run animation
    }
}