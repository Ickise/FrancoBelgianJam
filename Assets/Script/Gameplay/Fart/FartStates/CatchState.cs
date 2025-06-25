using UnityEngine;
using UnityEngine.AI;

public class CatchState : IFartState
{
    private FartController fart;
    private NavMeshAgent navMeshAgent;

    public CatchState(FartController fart)
    {
        this.fart = fart;
        navMeshAgent = fart.NavMeshAgent;
    }

    public void EnterState()
    {
        AudioManager.instance.PlaySFX("FartDisappearance");
        // Animation de capture
    }

    public void UpdateState()
    {
        Transform vacuum = fart.GetVacuumCatchArea();
        if (vacuum == null)
        {
            fart.SwitchState(new EvilState(fart));
            return;
        }

        navMeshAgent.SetDestination(vacuum.position);

        if (Vector3.Distance(fart.transform.position, vacuum.position) > fart.GetDistanceToBeEvil())
        {
            fart.SwitchState(new EvilState(fart));
        }
    }

    public void ExitState()
    {
    }
}