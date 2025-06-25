using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class CowController : MonoBehaviour
{
    public bool DebugGizmos => debugGizmos;
    public float ArrivalThreshold => arrivalThreshold;

    private ICowState currentState;
    
    [SerializeField] private float arrivalThreshold = 0.5f;
    [SerializeField] private int minDistance = 1;
    [SerializeField] private int maxDistance = 3;
    [SerializeField] private float speedPeace = 3f;
    [SerializeField] private int afraidDistance = 3;
    [SerializeField] private float speedAfraid = 5f;
    [SerializeField] private int fartDistance = 9;
    [SerializeField] private int minTimeRecoverFromFart = 5;
    [SerializeField] private int maxTimeRecoverFromFart = 9;
    [SerializeField] private int timeFartCreation = 2;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private int anticipationLevel = 1;
    [SerializeField] private bool debugGizmos = false;

    [SerializeField] private GameObject fartPrefab;

    [SerializeField] private Animator cowAnimator;

    private bool hasBeenScared = false;
    private bool hasBeenFarted = false;

    private Vector3 moveDirection;

    private void Start()
    {
        SwitchState(new PeaceState());

        if (!NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            Debug.LogError($"{name} is not on NavMesh at start! Relocate it.");
            return;
        }
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void SwitchState(ICowState newState, Vector3? dangerSource = null)
    {
        if (currentState?.GetType() == newState.GetType()) return;

        currentState?.ExitState();
        currentState = newState;

        if (dangerSource.HasValue)
            currentState.EnterState(this, dangerSource.Value);
        else
            currentState.EnterState(this);
    }

    public int GetMinDistance() => minDistance;
    public int GetMaxDistance() => maxDistance;
    public float GetSpeedPeace() => speedPeace;
    public int GetAfraidDistance() => afraidDistance;
    public float GetSpeedAfraid() => speedAfraid;
    public int GetFartDistance() => fartDistance;
    public int GetMinTimeRecoverFromFart() => minTimeRecoverFromFart;
    public int GetMaxTimeRecoverFromFart() => maxTimeRecoverFromFart;

    public bool HasBeenScared() => hasBeenScared;
    public bool HasBeenFarted() => hasBeenFarted;

    public int AnticipationLevel => anticipationLevel;

    public NavMeshAgent NavMeshAgent => navMeshAgent;

    public Animator Animator => cowAnimator;


    public void SpawnFart()
    {
        Vector3 spawnPosition = transform.position - transform.forward * 2f;
        Instantiate(fartPrefab, spawnPosition, Quaternion.identity, transform);
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

    public Vector3 GetEscapePosition(Vector3 from, float distance)
    {
        Vector3 dir = (transform.position - from).normalized;
        dir.y = 0;
        Vector3 offset = dir * anticipationLevel;
        Vector3 target = transform.position + dir * distance + offset;

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, distance, NavMesh.AllAreas))
            return hit.position;

        return transform.position;
    }
    
    public string GetCurrentStateName()
    {
        return currentState?.GetType().Name ?? "None";
    }

}

[ExecuteAlways]
[RequireComponent(typeof(CowController))]
public class CowGizmoDrawer : MonoBehaviour
{
    private CowController cow;

    private void Awake()
    {
        cow = GetComponent<CowController>();
    }

    private void OnDrawGizmos()
    {
        if (cow == null)
            cow = GetComponent<CowController>();

        if (!cow.DebugGizmos)
            return;

        var agent = cow.NavMeshAgent;
        if (agent == null || !agent.enabled || !agent.isOnNavMesh)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(cow.transform.position, agent.destination);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(agent.destination, 0.3f);

#if UNITY_EDITOR
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        Handles.Label(cow.transform.position + Vector3.up * 2f, $"STATE: {cow.GetCurrentStateName()}", style);
#endif
    }
}
