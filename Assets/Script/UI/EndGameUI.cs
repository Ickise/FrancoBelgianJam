using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI adaptativeText;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button tryAgainButton;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(GameManager.instance.MainMenu);
        tryAgainButton.onClick.AddListener(GameManager.instance.TryAgain);
    }

    private void OnEnable()
    {
        EndGameUIScreen();
    }

    private void EndGameUIScreen()
    {
        var playTime = GameManager.instance.GetPlayTime();

        eventSystem.SetSelectedGameObject(tryAgainButton.gameObject);
        var min = (int)(playTime / 60);
        var sec = (int)(playTime % 60);
        timerText.text = $"Score: {ScoreManager.instance.GetScore()} in {min} minutes and {sec} seconds.";

        if (GameManager.instance.HasPlayerWon())
        {
            titleText.text = "Victory";
            adaptativeText.text = "You supplied the city with enough gas to win!";
        }
        else if (GameManager.instance.HasPlayerLost())
        {
            titleText.text = "Defeat";
            adaptativeText.text = "Your battery ran out, and you failed to supply the city!";
        }
        else
        {
            titleText.text = "Game Over";
            adaptativeText.text = "Unexpected end condition.";
        }
    }
}