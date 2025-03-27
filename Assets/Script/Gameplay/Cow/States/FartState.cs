using UnityEngine;
using System.Collections;

public class FartState : ICowState
{
    private CowController cow;
    private Vector3 escapeDirection;
    private float remainingDistance;

    public void EnterState(CowController cow)
    {
    }

    public void EnterState(CowController cow, Vector3 dangerSource)
    {
        this.cow = cow;

        if (cow.HasBeenFarted()) return;

        escapeDirection = (cow.transform.position - dangerSource).normalized;
        remainingDistance = cow.GetFartDistance();
        cow.SetFarting(true);
        cow.SetFarted(true);

        cow.SpawnFart();
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
        cow.ResetFartState();
        cow.StopAllCoroutines();
    }

    private IEnumerator RecoverBeforePeace()
    {
        yield return new WaitForSeconds(Random.Range(cow.GetMinTimeRecoverFromFart(), cow.GetMaxTimeRecoverFromFart()));
        cow.SwitchState(new PeaceState());
    }
}