using UnityEngine;
using System.Collections;

public class FartState : ICowState
{
    private CowController cow;
    private Vector3 escapeDirection;
    private float remainingDistance;

    public void EnterState(CowController cow)
    {
        this.cow = cow;
        escapeDirection = -cow.GetMoveDirection();
        remainingDistance = cow.GetFartDistance();
        cow.SetFarting(true);
        cow.StartCoroutine(GenerateFarts());
    }

    public void UpdateState()
    {
        if (remainingDistance > 0)
        {
            float moveStep = cow.GetSpeedFart() * Time.deltaTime;
            cow.transform.position += escapeDirection * moveStep;
            remainingDistance -= moveStep;
        }
        else
        {
            cow.SetFarting(false);
            cow.StartCoroutine(RecoverBeforePeace());
        }
    }

    public void ExitState()
    {
        cow.SetFarting(false);
    }

    private IEnumerator GenerateFarts()
    {
        while (cow.IsFarting())
        {
            cow.SpawnFart();
            yield return new WaitForSeconds(cow.GetTimeFartCreation());
        }
    }

    private IEnumerator RecoverBeforePeace()
    {
        yield return new WaitForSeconds(Random.Range(cow.GetMinTimeRecoverFromFart(), cow.GetMaxTimeRecoverFromFart()));
        cow.SwitchState(new PeaceState());
    }
}