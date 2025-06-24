using System;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField, Header("References")] private InputReader inputReader;

    private Quaternion playerRotation;

    private void Update()
    {
        GetInputValue();
    }

    private void GetInputValue()
    {
        if (inputReader.Move != Vector2.zero)
        {
            float angle = Mathf.Atan2(inputReader.Move.x, inputReader.Move.y) * Mathf.Rad2Deg;
            playerRotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    public void RotatePlayer()
    {
        transform.rotation = playerRotation;
    }
}