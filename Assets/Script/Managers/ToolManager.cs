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
    }

    private void OnDisable()
    {
        inputReader.RightTriggerPressed -= () => SwitchTool(vacuum);
        inputReader.LeftTriggerPressed -= () => SwitchTool(soundMaker);
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

        Rigidbody activeRb = activeTool.GetComponent<Rigidbody>();
        lastPosition = activeTool.transform.position;
        lastVelocity = activeRb.linearVelocity;

        activeRb.linearVelocity = Vector3.zero;
        activeTool.gameObject.SetActive(false);

        Rigidbody newRb = newTool.GetComponent<Rigidbody>();
        newTool.transform.position = lastPosition;
        newRb.linearVelocity = lastVelocity;
        newTool.gameObject.SetActive(true);

        activeTool = newTool;
    }
}