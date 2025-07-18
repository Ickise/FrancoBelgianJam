using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Other Script References")] [SerializeField]
    private GasManager gasManagerRef;

    [SerializeField] private BatteryManager batteryManagerRef;
    [SerializeField] private UpgradeManager upgradeManagerRef;

    [Header("Data References")] [SerializeField]
    private InputReader inputReader;

    [Header("Player References")] [SerializeField]
    private Transform playerTransform;

    [SerializeField] private Transform vacuumTransform;

    [Header("Game Settings")] [SerializeField]
    private float gasToDepositToWin = 100;

    private float playTime;

    public int currentGasStock;

    public GasManager GasManagerRef => gasManagerRef;
    public BatteryManager BatteryManagerRef => batteryManagerRef;
    public  UpgradeManager UpgradeManagerRef => upgradeManagerRef;

    public InputReader InputReader => inputReader;

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

        inputReader.EnablePlayerInputs();
    }

    private void Update()
    {
        playTime += Time.deltaTime;
    }

    public float GetPlayTime()
    {
        return playTime;
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
        AudioManager.instance.Play("MainTheme");
    }

    public void Reset()
    {
        SceneManager.LoadScene("Menu");
    }

    public float GetGasThreshold()
    {
        return gasToDepositToWin;
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

    public Transform GetVacuumTransform()
    {
        return vacuumTransform;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    
    public bool HasPlayerWon()
    {
        return currentGasStock >= GetGasThreshold();
    }

    public bool HasPlayerLost()
    {
        return BatteryManagerRef.GetCurrentBattery() <= 0;
    }
    
    public void TryAgain()
    {
        Time.timeScale = 1;
        var currentScene = SceneManager.GetActiveScene();
        
        SceneManager.LoadScene(currentScene.name);
    }
}