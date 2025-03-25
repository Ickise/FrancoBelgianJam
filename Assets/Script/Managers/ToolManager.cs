using UnityEngine;

public class ToolManager : MonoBehaviour
{
    [SerializeField, Header("References")] private InputReader inputReader;
    [SerializeField] private ToolBase vacuum;
    [SerializeField] private ToolBase soundMaker;

    private ToolBase activeTool;

    private Vector3 lastPosition;
    private Vector3 lastVelocity;

    private void OnEnable()
    {
        inputReader.RightTriggerPressed += () => SwitchTool(vacuum);
        inputReader.LeftTriggerPressed += () => SwitchTool(soundMaker);
        inputReader.ActionButtonPressed += () => activeTool?.UseTool(false);
        inputReader.ActionButtonHeld += () => activeTool?.UseTool(true);
    }

    private void OnDisable()
    {
        inputReader.RightTriggerPressed -= () => SwitchTool(vacuum);
        inputReader.LeftTriggerPressed -= () => SwitchTool(soundMaker);
        inputReader.ActionButtonPressed -= () => activeTool?.UseTool(false);
        inputReader.ActionButtonHeld -= () => activeTool?.UseTool(true);
    }

    private void Start()
    {
        activeTool = soundMaker;
        soundMaker.gameObject.SetActive(true);
        vacuum.gameObject.SetActive(false);
    }

    private void SwitchTool(ToolBase newTool)
    {
        if (activeTool == newTool) return;

        activeTool.gameObject.SetActive(false);
        newTool.transform.position = activeTool.transform.position;
        newTool.gameObject.SetActive(true);
        activeTool = newTool;
    }
}