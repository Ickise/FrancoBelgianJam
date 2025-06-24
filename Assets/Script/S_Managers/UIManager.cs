using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    [Header("Data References")] [SerializeField]
    private InputReader inputReader;
    
    [Header("Canvas References")] [SerializeField]
    private GameObject inGameCanvas;
    [SerializeField] private GameObject upgradeCanvas;
    
    [Header("Other Script References")] [SerializeField]
    private GasUI gasUI;
    [SerializeField] private BatteryUI batteryUI;
    
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
        gasUI.UpdateGasUI();
        batteryUI.UpdateEnergyUI();
        
        ShowInGameCanvas();
    }

    private void ShowInGameCanvas()
    {
        SetActiveCanvas(inGameCanvas);

        if (!inGameCanvas.activeSelf) return;

        MouseManager.DisableCursor();
        inputReader.EnablePlayerInputs();
        Time.timeScale = 1f;
    }

    private void ShowUpgradeCanvas()
    {
        SetActiveCanvas(upgradeCanvas);

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
    }
}
