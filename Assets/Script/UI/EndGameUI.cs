using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private float gasToDepositToWin = 250;

    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(GameManager.instance.MainMenu);
    }

    private void OnEnable()
    {
        EndGameUIScreen(GameManager.instance.GasManagerRef.GetGasStock());
    }

    public void EndGameUIScreen(float currentGas)
    {
        if (!(currentGas >= gasToDepositToWin)) return;

        var playTime = GameManager.instance.GetPlayTime();
        
        eventSystem.SetSelectedGameObject(mainMenuButton.gameObject);
        var min = (int)(playTime / 60);
        var sec = (int)(playTime % 60);
        endGameText.text = $"Score: {ScoreManager.instance.GetScore()} in {min} minutes and {sec} seconds.";
    }
}