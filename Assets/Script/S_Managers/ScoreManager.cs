using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    [SerializeField, Header("References")] private UIScore uiScore;

    private int score = 0;
    
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
    
    public void ChangeScoreValue(int amount, bool isIncreasing)
    {
        score = isIncreasing ? score + amount : score - amount;
        uiScore.UpdateScoreUI();
    }
    
    public int GetScore()
    {
        return score;
    }
}
