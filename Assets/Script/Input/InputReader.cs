using UnityEngine.InputSystem;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
{
    public event Action RightTriggerPressed = delegate { };
    public event Action LeftTriggerPressed = delegate { };
    public event Action ActionButtonPressed = delegate { };
    public event Action ActionButtonHeld = delegate { };
    public event Action<Vector2> AimMovementEvent = delegate { };

    private PlayerInputActions inputActions;

    public bool RightTriggerIsPressed => inputActions.Player.Vaccum.IsPressed();
    public bool LeftTriggerIsPressed => inputActions.Player.MakeSound.IsPressed();

    private void OnEnable()
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

    private void HandleToolInput(InputAction.CallbackContext context, Action triggerAction)
    {
        if (context.started)
        {
            ActionButtonHeld.Invoke();
        }
        else if (context.canceled)
        {
            ActionButtonPressed.Invoke();
        }

        if (!context.performed)
        {
            return;
        }

        triggerAction.Invoke();
    }

    public void OnMakeSound(InputAction.CallbackContext context)
    {
        HandleToolInput(context, LeftTriggerPressed);
    }

    public void OnVaccum(InputAction.CallbackContext context)
    {
        HandleToolInput(context, RightTriggerPressed);
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
}