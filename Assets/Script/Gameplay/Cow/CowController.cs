using UnityEngine;

public class CowController : MonoBehaviour
{
    private ICowState currentState;

    [SerializeField] private int minTimeStatic = 3;
    [SerializeField] private int maxTimeStatic = 5;
    [SerializeField] private int minDistance = 1;
    [SerializeField] private int maxDistance = 3;
    [SerializeField] private float speedPeace = 3f;
    [SerializeField] private int afraidDistance = 3;
    [SerializeField] private float speedAfraid = 5f;
    [SerializeField] private int fartDistance = 9;
    [SerializeField] private float speedFart = 9f;
    [SerializeField] private int minTimeRecoverFromFart = 5;
    [SerializeField] private int maxTimeRecoverFromFart = 9;
    [SerializeField] private int timeFartCreation = 2;

    [SerializeField] private GameObject fartPrefab;
    
    private bool hasBeenScared = false;
    private bool hasBeenFarted = false;
    
    private Vector3 moveDirection;
    private bool isFarting = false;

    private void Start()
    {
        SwitchState(new PeaceState());
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void SwitchState(ICowState newState, Vector3? dangerSource = null)
    {
        currentState?.ExitState();
        currentState = newState;
        if (dangerSource.HasValue)
        {
            currentState.EnterState(this, dangerSource.Value);
        }
        else
        {
            currentState.EnterState(this);
        }
    }

    public int GetMinTimeStatic() => minTimeStatic;
    public int GetMaxTimeStatic() => maxTimeStatic;
    public int GetMinDistance() => minDistance;
    public int GetMaxDistance() => maxDistance;
    public float GetSpeedPeace() => speedPeace;
    public int GetAfraidDistance() => afraidDistance;
    public float GetSpeedAfraid() => speedAfraid;
    public int GetFartDistance() => fartDistance;
    public float GetSpeedFart() => speedFart;
    public int GetMinTimeRecoverFromFart() => minTimeRecoverFromFart;
    public int GetMaxTimeRecoverFromFart() => maxTimeRecoverFromFart;
    public int GetTimeFartCreation() => timeFartCreation;
    
    public bool HasBeenScared() => hasBeenScared;
    public bool HasBeenFarted() => hasBeenFarted;

    public void SetMoveDirection(Vector3 direction) => moveDirection = direction;
    public Vector3 GetMoveDirection() => moveDirection;
    public bool IsFarting() => isFarting;
    public void SetFarting(bool value) => isFarting = value;
    
    public void SpawnFart()
    {
        Vector3 spawnPosition = transform.position - transform.forward * 2f;
        Instantiate(fartPrefab, spawnPosition, Quaternion.identity);
    }
    
    public void ResetScaredState()
    {
        hasBeenScared = false;
    }
    
    public void SetScared(bool state)
    {
        hasBeenScared = state;
    }

    public void ResetFartState()
    {
        hasBeenFarted = false;
    }
    
    public void SetFarted(bool state)
    {
        hasBeenFarted = state;
    }
}