using System;
using UnityEngine;

public class ToolsManager : MonoBehaviour
{
    [SerializeField, Header("References")] private InputReader inputReader;
    [SerializeField] private GameObject vacuum;
    [SerializeField] private GameObject soundMaker;

    private void OnEnable()
    {
        inputReader.RightTriggerPressed += AppearVacuum;
        inputReader.LeftTriggerPressed += AppearSoundMaker;
    }

    private void OnDisable()
    {
        inputReader.RightTriggerPressed -= AppearVacuum;
        inputReader.LeftTriggerPressed -= AppearSoundMaker;
    }

    private void Start()
    {
        AppearSoundMaker();
    }

    private void AppearVacuum()
    {
        vacuum.SetActive(true);
        soundMaker.SetActive(false);
    }
    
    private void AppearSoundMaker()
    {
        soundMaker.SetActive(true);
        vacuum.SetActive(false);
    }
}
