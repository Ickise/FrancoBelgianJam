using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Header("References")] private Rigidbody playerRigidbody;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerRotation playerRotation;
    [SerializeField] private Animator animator;
    
    [SerializeField, Header("Settings")] private float speed = 3f;
    [SerializeField] private float overchargeSpeed = 5f;
    [SerializeField] private float overfillSpeedRate = 0.75f;
    [SerializeField] private float moveConsumption = 1f;
    [SerializeField] private float moveConsumptionRate = 1f;

    private Vector3 movement;

    private float time;

    private float overfillSpeed;

    private BatteryManager batteryManager;

    private GasManager gasManager;

    private void OnEnable()
    {
        inputReader.MovementEvent += GetInputValue;
    }

    private void OnDisable()
    {
        inputReader.MovementEvent -= GetInputValue;
    }

    private void GetInputValue(Vector2 direction)
    {
        movement = new Vector3(direction.x, 0, direction.y).normalized;
    }

    private void Start()
    {
        batteryManager = BatteryManager.instance;
        gasManager = GasManager.instance;
        overfillSpeed = speed * overfillSpeedRate;
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (movement != Vector3.zero)
        {
            time += Time.fixedDeltaTime;

            if (time >= moveConsumptionRate)
            {
                batteryManager.ChangeEnergyValue(moveConsumption, false);
                time = 0;
            }
        }
    }

    private void MovePlayer()
    {
        var currentSpeed = batteryManager.BatteryOvercharging() ? overchargeSpeed : speed;
        Vector3 velocity = playerRigidbody.linearVelocity;

        velocity.x = movement.x * (gasManager.IsGasStockOverFilled() ? overfillSpeed : currentSpeed);
        velocity.z = movement.z * (gasManager.IsGasStockOverFilled() ? overfillSpeed : currentSpeed);

        playerRigidbody.linearVelocity = velocity;
        
        if (movement != Vector3.zero)
        {
            animator.SetTrigger("IsWalking");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
        
        playerRotation.RotatePlayer();
    }
}