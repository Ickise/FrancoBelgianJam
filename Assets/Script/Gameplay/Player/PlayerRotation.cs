using System;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private Quaternion playerRotation;

    private InputReader inputReader;

    private void Awake()
    {
        inputReader = GameManager.instance?.InputReader;
    }

    private void Update()
    {
        GetInputValue();
    }
    
    private void GetInputValue()
    {
        if (inputReader.Move == Vector2.zero) return;
        
        var angle = Mathf.Atan2(inputReader.Move.x, inputReader.Move.y) * Mathf.Rad2Deg;
        playerRotation = Quaternion.Euler(0f, angle, 0f);
    }

    public void RotatePlayer()
    {
        transform.rotation = playerRotation;
    }
}