using UnityEngine;

public class FartController : MonoBehaviour
{
    private IFartState currentState;

    [SerializeField] private float speedFollowCow = 3f;
    [SerializeField] private float timeFollowCow = 3f;
    [SerializeField] private int rangeAngle = 45;
    [SerializeField] private float evilSpeed = 3f;
    [SerializeField] private float timeToDisappear = 5f;
    [SerializeField] private float fleeSpeed = 3f;
    [SerializeField] private float catchSpeed = 5f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float distanceToBeEvil = 10f;
    
    private Vector3 playerDir;
    private Vector3 cowPosition;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        cowPosition = GetComponentInParent<Transform>().position;
        SwitchState(new EvilState(this));
    }

    private void Update()
    {
        currentState?.UpdateState();
        UpdatePlayerDirection();
    }


    private void UpdatePlayerDirection()
    {
        if (playerTransform != null)
        {
            playerDir = (gameManager.GetPlayerTransform().position - transform.position).normalized;
        }
    }

    public void SwitchState(IFartState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public Vector3 GetCowPosition() => cowPosition;
    public Vector3 GetPlayerDirection() => playerDir;
    public Transform GetVacuumAttractiveArea() => gameManager.GetVacuumTransform();
    public Transform GetVacuumCatchArea() => gameManager.GetVacuumTransform();

    public float GetSpeedFollowCow() => speedFollowCow;
    public float GetTimeFollowCow() => timeFollowCow;
    public float GetEvilSpeed() => evilSpeed;
    public float GetTimeToDisappear() => timeToDisappear;
    public float GetFleeSpeed() => fleeSpeed;
    public float GetCatchSpeed() => catchSpeed;
    public float GetDistanceToBeEvil() => distanceToBeEvil;

    public int GetRangeAngle() => rangeAngle;
}