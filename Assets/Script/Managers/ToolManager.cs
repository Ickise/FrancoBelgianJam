using UnityEngine;

public class ToolManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private ToolBase vacuum;
    [SerializeField] private ToolBase soundMaker;

    private ToolBase activeTool;
    private bool canUseTool = true;

    private void OnEnable()
    {
        inputReader.RightTriggerEvent += OnRightTriggerPressed;
        inputReader.LeftTriggerEvent += OnLeftTriggerPressed;
        inputReader.RightTriggerEvent += () => HandleToolUsage(true);
        inputReader.LeftTriggerEvent += () => HandleToolUsage(true);
    }

    private void OnDisable()
    {
        inputReader.RightTriggerEvent -= OnRightTriggerPressed;
        inputReader.LeftTriggerEvent -= OnLeftTriggerPressed;
        inputReader.RightTriggerEvent -= () => HandleToolUsage(true);
        inputReader.LeftTriggerEvent -= () => HandleToolUsage(true);
    }

    private void Start()
    {
        activeTool = soundMaker;
        soundMaker.gameObject.SetActive(true);
        vacuum.gameObject.SetActive(false);
    }

    private void OnRightTriggerPressed()
    {
        SwitchTool(vacuum);
    }

    private void OnLeftTriggerPressed()
    {
        SwitchTool(soundMaker);
    }

    private void SwitchTool(ToolBase newTool)
    {
        if (activeTool == newTool) return;

        activeTool.gameObject.SetActive(false);
        newTool.gameObject.SetActive(true);
        activeTool = newTool;

        canUseTool = false;
        Invoke(nameof(EnableToolUsage), 0.4f);
    }

    private void EnableToolUsage()
    {
        canUseTool = true;
    }

    private void HandleToolUsage(bool isHeld)
    {
        if (canUseTool)
        {
            activeTool?.UseTool(isHeld);
        }
    }
}