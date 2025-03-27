using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField, Header("References")] private InputReader inputReader;
    
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform vacuumTransform;

    [SerializeField] private float gasToDepositToWin = 700;
    [SerializeField, Header("References")] private GameObject victoryScreen;
    [SerializeField, Header("References")] private TextMeshProUGUI victoryTextTime;
    [SerializeField, Header("References")] private GameObject defeatScreen;
    [SerializeField, Header("References")] private GameObject inGameScreen;

    private float _playTime;
    
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
    
    public void GameOver()
    {
        Time.timeScale = 0;
        inGameScreen.SetActive(false);
        defeatScreen.SetActive(true);
    }
    
    public void Victory(float currentGas)
    {
        if (currentGas >= gasToDepositToWin)
        {
            Time.timeScale = 0;
            inGameScreen.SetActive(false);
            victoryScreen.SetActive(true);

            var min = (int)(_playTime / 60);
            var sec = (int)(_playTime % 60);
            victoryTextTime.text = $"{min} minutes and {sec} seconds.";
        }
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
}