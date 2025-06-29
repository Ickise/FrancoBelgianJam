using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Data References")] [SerializeField]
    private InputReader inputReader;

    [Header("Canvas References")] [SerializeField]
    private GameObject inGameCanvas;

    [SerializeField] private GameObject upgradeCanvas;
    [SerializeField] private GameObject endGameCanvas;

    [Header("Other Script References")] [SerializeField]
    private GasUI gasUI;

    [SerializeField] private BatteryUI batteryUI;
    [SerializeField] private EndGameUI endGameUI;

    public GasUI GasUI => gasUI;
    public BatteryUI BatteryUI => batteryUI;
    public EndGameUI EndGameUI => endGameUI;

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

    private void Start()
    {
        ShowInGameCanvas();

        gasUI.UpdateGasUI();
        batteryUI.InitializeBatteryUI();
    }

    private void ShowInGameCanvas()
    {
        SetActiveCanvas(inGameCanvas);

        if (!inGameCanvas.activeSelf) return;

        MouseManager.DisableCursor();
        inputReader.EnablePlayerInputs();
        Time.timeScale = 1f;
    }

    public void ShowUpgradeCanvas()
    {
        ToggleCanvas(upgradeCanvas);

        if (upgradeCanvas.activeSelf)
        {
            MouseManager.EnableCursor();
            inputReader.DisablePlayerInputs();
            Time.timeScale = 0f;
        }
        else
        {
            ShowInGameCanvas();
        }
    }

    public void ShowEndGameCanvas()
    {
        SetActiveCanvas(endGameCanvas);

        if (!endGameCanvas.activeSelf) return;

        MouseManager.EnableCursor();
        inputReader.DisablePlayerInputs();
        Time.timeScale = 0f;
    }

    private void SetActiveCanvas(GameObject activeCanvas)
    {
        DisableCanvas();
        activeCanvas.SetActive(true);
    }

    private void ToggleCanvas(GameObject canvas)
    {
        canvas.SetActive(!canvas.activeSelf);
    }

    private void DisableCanvas()
    {
        inGameCanvas.SetActive(false);
        upgradeCanvas.SetActive(false);
        endGameCanvas.SetActive(false);
    }
}