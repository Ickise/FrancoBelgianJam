using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Other Script References")] [SerializeField]
    private GasManager gasManagerRef;

    [SerializeField] private BatteryManager batteryManagerRef;

    [Header("Data References")] [SerializeField]
    private InputReader inputReader;

    [Header("Player References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform vacuumTransform;

    [Header("Game Settings")]
    [SerializeField] private float gasToDepositToWin = 250;
   
    private float playTime;

    public GasManager GasManagerRef => gasManagerRef;
    public BatteryManager BatteryManagerRef => batteryManagerRef;

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
}