using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    [SerializeField, Header("References")] private TextMeshProUGUI scoreText;

    [SerializeField] private ScoreManager scoreManager;

    private void Start()
    {
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        scoreText.text = $"Score stock: {scoreManager.GetScore()}";
    }
}