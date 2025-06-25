using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class InputDeviceManager : MonoBehaviour
{
    [SerializeField] private bool debugLogs = true;

    private bool isGamepadActive = false;

    private void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (eventPtr.type != StateEvent.Type && eventPtr.type != DeltaStateEvent.Type) return;

        if (device is Gamepad)
        {
            if (isGamepadActive) return;

            isGamepadActive = true;
            MouseManager.DisableCursor();

            if (debugLogs) Debug.Log("Gamepad active");
        }
        else if (device is Mouse || device is Keyboard)
        {
            if (!isGamepadActive) return;

            isGamepadActive = false;
            MouseManager.EnableCursor();

            if (debugLogs) Debug.Log("Mouse or keyboard active");
        }
    }
}