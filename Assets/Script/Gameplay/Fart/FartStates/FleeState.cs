using UnityEngine;

public class FleeState : IFartState
{
    private FartController fart;

    public FleeState(FartController fart)
    {
        this.fart = fart;
    }

    public void EnterState()
    {
        // Flee animation
    }

    public void UpdateState()
    {
        Transform vacuum = fart.GetVacuumAttractiveArea();
        if (vacuum == null)
        {
            fart.SwitchState(new EvilState(fart));
            return;
        }

        Vector3 fleeDirection = (fart.transform.position - vacuum.position).normalized;
        fart.transform.position += fleeDirection * fart.GetFleeSpeed() * Time.deltaTime;

        if (Vector3.Distance(fart.transform.position, vacuum.position) > fart.GetDistanceToBeEvil())
        {
            fart.SwitchState(new EvilState(fart));
        }
    }

    public void ExitState()
    {
    }
}