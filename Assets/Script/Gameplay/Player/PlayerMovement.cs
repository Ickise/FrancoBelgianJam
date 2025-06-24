using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Other Script References")] [SerializeField]
    private PlayerRotation playerRotation;

    [Header("Data References")] [SerializeField]
    private PlayerData playerData;

    [Header("Player Component References")] [SerializeField]
    private Animator animator;

    [SerializeField] private CharacterController cont;

    [Header("Player Effect References")] [SerializeField]
    private ParticleSystem smokeEffect;

    private float speedMultiplier = 1;
    private float overfillSpeed;
    private float time;

    private BatteryManager batteryManager;
    private InputReader inputReader;
    private GasManager gasManager;

    private Vector3 velocity;

    private void Awake()
    {
        inputReader = GameManager.instance?.InputReader;
    }

    private void Start()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        batteryManager = GameManager.instance?.BatteryManagerRef;
        gasManager = GameManager.instance?.GasManagerRef;
        overfillSpeed = playerData.speed * playerData.overfillSpeedRate;

        smokeEffect.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        HandleMovement();
        ApplyCustomGravity();
    }

    private void ApplyCustomGravity()
    {
        velocity.y = -2f;

        cont.Move(velocity * Time.deltaTime);
    }

    private void HandleMovement()
    {
        Vector2 inputDirection = inputReader.Move;

        var currentSpeed = GetCurrentSpeed();

        Vector3 move = CalculateMovement(inputDirection, currentSpeed);

        HandleAnimationAndEffects(move);

        playerRotation.RotatePlayer();

        cont.Move(move * (playerData.speed * Time.deltaTime));

        HandleEnergyConsumption(move);
    }

    private float GetCurrentSpeed()
    {
        return batteryManager.BatteryOvercharging()
            ? playerData.overchargeSpeed * speedMultiplier
            : playerData.speed * speedMultiplier;
    }

    private Vector3 CalculateMovement(Vector2 inputDirection, float currentSpeed)
    {
        var isGasOverfilled = gasManager.IsGasStockOverFilled();
        var effectiveSpeed = isGasOverfilled ? overfillSpeed * speedMultiplier : currentSpeed;

        var move = transform.forward * inputDirection.y + transform.right * inputDirection.x;
        move.x = inputDirection.x * effectiveSpeed;
        move.z = inputDirection.y * effectiveSpeed;
        move.y = velocity.y;

        return move.normalized;
    }

    private void HandleAnimationAndEffects(Vector3 move)
    {
        if (move.x != 0 || move.z != 0)
        {
            animator.SetTrigger("IsWalking");

            smokeEffect.gameObject.SetActive(true);

            if (!smokeEffect.isPlaying)
            {
                smokeEffect.Play();
            }
            // WalkAudioFeedback();
        }
        else
        {
            animator.SetTrigger("Idle");

            smokeEffect.Stop();

            if (smokeEffect.isStopped)
            {
                smokeEffect.gameObject.SetActive(false);
            }
        }
    }

    private void HandleEnergyConsumption(Vector3 move)
    {
        if (move == Vector3.zero) return;

        time += Time.fixedDeltaTime;

        if (!(time >= playerData.moveConsumptionRate)) return;

        batteryManager.ChangeEnergyValue(playerData.moveConsumption, false);
        time = 0;
    }

    public void ChangeSpeedMultiplier(float value)
    {
        speedMultiplier = value;
    }

    public void ChangeOverfillSpeed(float value)
    {
        playerData.overfillSpeedRate = value;
        overfillSpeed = playerData.speed * playerData.overfillSpeedRate;
    }
}