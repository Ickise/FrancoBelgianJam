using UnityEngine;

public abstract class ToolBase : MonoBehaviour
{
    [SerializeField, Header("References")] private Transform toolTransform;
    [SerializeField] private Rigidbody toolRigidbody;
    [SerializeField] private InputReader inputReader;

    [SerializeField, Header("Settings")] private float speed = 3f;

    private Vector3 movement;

    protected void OnEnable()
    {
        inputReader.AimMovementEvent += GetInputValue;
    }

    protected void OnDisable()
    {
        inputReader.AimMovementEvent -= GetInputValue;
        toolRigidbody.linearVelocity = Vector3.zero;
    }

    protected void FixedUpdate()
    {
        MoveTool();
    }

    protected void GetInputValue(Vector2 direction)
    {
        movement = new Vector3(direction.x, 0, direction.y);
    }

    protected void MoveTool()
    {
        toolRigidbody.linearVelocity = movement * speed;
    }
}