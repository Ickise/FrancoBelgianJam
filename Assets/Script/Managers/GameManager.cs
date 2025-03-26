using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField, Header("References")] private InputReader inputReader;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        inputReader.EnablePlayerInputs();
    }

    private void OnDisable()
    {
        inputReader.DisablePlayerInputs();
    }
    
    public void GameOver()
    {
        Debug.Log("Game Over");
    }
}