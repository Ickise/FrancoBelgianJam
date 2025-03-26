using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Header("References")] private Rigidbody playerRigidbody;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerRotation playerRotation;
    
    [SerializeField, Header("Settings")]
    private float speed = 3f;

    private Vector3 movement;

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
    }

    private void MovePlayer()
    {
        playerRigidbody.linearVelocity = movement * speed;
        playerRotation.RotatePlayer();
    }
}