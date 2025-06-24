using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
{
    public Vector2 Move => inputActions.Player.Movement.ReadValue<Vector2>();

    public event Action RightTriggerEvent = delegate { };
    public event Action LeftTriggerEvent = delegate { };
    public event Action AnyTriggerHeld = delegate { };

    private PlayerInputActions inputActions;

    public bool RightTriggerIsPressed => inputActions.Player.Vaccum.IsPressed();
    public bool LeftTriggerIsPressed => inputActions.Player.MakeSound.IsPressed();

    private void OnEnable()
    {
        if (inputActions != null) return;
        
        inputActions = new PlayerInputActions();
        inputActions.Player.SetCallbacks(this);
    }

    public void EnablePlayerInputs() => inputActions.Enable();
    public void DisablePlayerInputs() => inputActions.Disable();

    public void EnablePlayerMovement() => inputActions.Player.Enable();
    public void DisablePlayerMovement() => inputActions.Player.Disable();

    private void HandleTriggerInput(InputAction.CallbackContext context, Action heldAction)
    {
        if (!context.performed) return;

        heldAction.Invoke();
        AnyTriggerHeld.Invoke();
    }

    public void OnMakeSound(InputAction.CallbackContext context)
    {
        if (RightTriggerIsPressed) return;
        HandleTriggerInput(context, LeftTriggerEvent);
    }

    public void OnVaccum(InputAction.CallbackContext context)
    {
        if (LeftTriggerIsPressed) return;
        HandleTriggerInput(context, RightTriggerEvent);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        //Noop
    }
}