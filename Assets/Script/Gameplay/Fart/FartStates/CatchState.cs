using UnityEngine;

public class CatchState : IFartState
{
    private FartController fart;

    public CatchState(FartController fart)
    {
        this.fart = fart;
    }

    public void EnterState()
    {
        // Catched animation
    }

    public void UpdateState()
    {
        Transform vacuum = fart.GetVacuumCatchArea();
        if (vacuum == null)
        {
            fart.SwitchState(new EvilState(fart));
            return;
        }

        fart.transform.position = Vector3.MoveTowards(fart.transform.position, vacuum.position, fart.GetCatchSpeed() * Time.deltaTime);

        if (Vector3.Distance(fart.transform.position, vacuum.position) > fart.GetDistanceToBeEvil())
        {
            fart.SwitchState(new EvilState(fart));
        }
    }

    public void ExitState() { }
}