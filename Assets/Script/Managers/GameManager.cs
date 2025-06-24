using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField, Header("References")] private InputReader inputReader;
    [SerializeField] private GasManager gasManagerRef;
    [SerializeField] private BatteryManager batteryManagerRef;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform vacuumTransform;

    [SerializeField] private float gasToDepositToWin = 250;
    [SerializeField, Header("References")] private GameObject victoryScreen;
    [SerializeField] private TextMeshProUGUI victoryTextTime;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private TextMeshProUGUI defeatScore;
    [SerializeField] private GameObject inGameScreen;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject firstVictoryButton;
    [SerializeField] private GameObject firstDefeatButton;
    private float _playTime;

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
        _playTime += Time.deltaTime;
    }

    void ResetTimer()
    {
        _playTime = 0;
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
        Time.timeScale = 0;
        inGameScreen.SetActive(false);
        defeatScreen.SetActive(true);
        eventSystem.SetSelectedGameObject(firstDefeatButton);
        defeatScore.text = $"Score: {ScoreManager.instance.GetScore()}";
    }

    public void Victory(float currentGas)
    {
        if (currentGas >= gasToDepositToWin)
        {
            Time.timeScale = 0;
            inGameScreen.SetActive(false);
            victoryScreen.SetActive(true);
            eventSystem.SetSelectedGameObject(firstVictoryButton);
            var min = (int)(_playTime / 60);
            var sec = (int)(_playTime % 60);
            victoryTextTime.text = $"{min} minutes and {sec} seconds.";
        }
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