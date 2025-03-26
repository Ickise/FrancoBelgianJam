using UnityEngine;

public class ToolManager : MonoBehaviour
{
    [SerializeField, Header("References")] private InputReader inputReader;
    [SerializeField] private ToolBase vacuum;
    [SerializeField] private ToolBase soundMaker;

    private ToolBase activeTool;

    private Vector3 lastPosition;
    private Vector3 lastVelocity;
    
    private bool canUseTool = true; 

    private void OnEnable()
    {
        inputReader.RightTriggerPressed += () => SwitchTool(vacuum);
        inputReader.LeftTriggerPressed += () => SwitchTool(soundMaker);
        inputReader.ActionButtonPressed += () => HandleToolUsage(false);
        inputReader.ActionButtonHeld += () => HandleToolUsage(true);
    }

    private void OnDisable()
    {
        inputReader.RightTriggerPressed -= () => SwitchTool(vacuum);
        inputReader.LeftTriggerPressed -= () => SwitchTool(soundMaker);
        inputReader.ActionButtonPressed -= () => HandleToolUsage(false);
        inputReader.ActionButtonHeld -= () => HandleToolUsage(true);
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