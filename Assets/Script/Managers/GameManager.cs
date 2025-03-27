using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField, Header("References")] private InputReader inputReader;
    
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform vacuumTransform;

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

    private void Start()
    {
        AudioManager.instance.PlayMusic(3, true);
    }

    public void GameOver()
    {
        //Debug.Log("Game Over");
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }
    
    public Transform GetVacuumTransform()
    {
        return vacuumTransform;
    }
}