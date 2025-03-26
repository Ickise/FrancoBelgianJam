using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Header("References")] private Rigidbody playerRigidbody;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerRotation playerRotation;

    [SerializeField, Header("Settings")] private float speed = 3f;
    [SerializeField] private float overchargeSpeed = 5f;
    [SerializeField] private float moveConsumption = 1f;
    [SerializeField] private float moveConsumptionRate = 1f;

    private Vector3 movement;

    private float time;

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
        movement = new Vector3(direction.x, 0, direction.y);
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (movement != Vector3.zero)
        {
            time += Time.fixedDeltaTime;

            if (time >= moveConsumptionRate)
            {
                BatteryManager.instance.ChangeEnergyValue(moveConsumption, false);
                time = 0;
            }
        }
    }

    private void MovePlayer()
    {
        var currentSpeed = BatteryManager.instance.BatteryOvercharging() ? overchargeSpeed : speed;

        playerRigidbody.linearVelocity = movement * currentSpeed;
        playerRotation.RotatePlayer();
    }
}