using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField, Header("References")] private InputReader inputReader;

    private Quaternion playerRotation;

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
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            playerRotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    public void RotatePlayer()
    {
        transform.rotation = playerRotation;
    }
}