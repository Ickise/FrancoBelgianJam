public interface ICowState
{
    void EnterState(CowController cow);
    void UpdateState();
    void ExitState();
}