using UnityEngine;
using UnityEngine.Serialization;

public abstract class ToolBase : MonoBehaviour
{
    [SerializeField, Header("References")] private Transform toolTransform;
    [SerializeField] private Rigidbody toolRigidbody;
    [SerializeField] private InputReader inputReader;

    [SerializeField, Header("Settings")] private float speed = 3f;

    private Vector3 movement;

    private void OnEnable()
    {
        inputReader.AimMovementEvent += GetInputValue;
    }

    private void OnDisable()
    {
        inputReader.AimMovementEvent -= GetInputValue;
        toolRigidbody.linearVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        MoveTool();
    }

    private void GetInputValue(Vector2 direction)
    {
        movement = new Vector3(direction.x, 0, direction.y);
    }

    private void MoveTool()
    {
        toolRigidbody.linearVelocity = movement * speed;
    }
}