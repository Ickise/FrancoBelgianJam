using UnityEngine.InputSystem;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
{
    public event Action RightTriggerPressed = delegate { };
    public event Action LeftTriggerPressed = delegate { };
    public event Action<Vector2> AimMovementEvent = delegate { };

    // public bool rightTriggerIsPressed => inputActions.Player.Vaccum.IsPressed();
    // public bool leftTriggerIsPressed => inputActions.Player.MakeSound.IsPressed();

    private PlayerInputActions inputActions;

    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerInputActions();
            inputActions.Player.SetCallbacks(this);
        }
    }

    public void EnablePlayerInputs()
    {
        inputActions.Enable();
    }

    public void DisablePlayerInputs()
    {
        inputActions.Disable();
    }

    public void OnMakeSound(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        LeftTriggerPressed.Invoke();
    }

    public void OnVaccum(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        RightTriggerPressed.Invoke();
    }

    public void OnAimMovement(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            AimMovementEvent.Invoke(Vector2.zero);
            return;
        }

        AimMovementEvent.Invoke(inputActions.Player.AimMovement.ReadValue<Vector2>());
    }

    public bool RightTriggerIsPressed => inputActions.Player.Vaccum.IsPressed();
    public bool LeftTriggerIsPressed => inputActions.Player.MakeSound.IsPressed();
}