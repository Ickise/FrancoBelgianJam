using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    [SerializeField, Header("References")] private TextMeshProUGUI scoreText;
   
    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = ScoreManager.instance;
        UpdateScoreUI();
    }
   
    public void UpdateScoreUI()
    {
        scoreText.text = $"Score stock: {scoreManager.GetScore()}";
    }
}
