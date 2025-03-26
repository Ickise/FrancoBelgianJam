using UnityEngine;

public class AirHorn : ToolBase
{
    [SerializeField, Header("References")] private GameObject littleDisturbPrefab;
    [SerializeField] private GameObject bigDisturbPrefab;

    [SerializeField] private InputReader inputReader;

    [SerializeField, Header("Settings")] private float holdThreshold = 3f;
    [SerializeField] private float littleDisturbAreaRange = 60f;
    [SerializeField] private float littleDisturbAreaDistance = 3f;
    [SerializeField] private float bigDisturbAreaRange = 90f;
    [SerializeField] private float bigDisturbAreDistance = 5f;
    [SerializeField] private float bigDisturbAreaTimeLife = 2f;
    [SerializeField] private float recoverTime = .5f;
    [SerializeField] private float radius = 3f;

    [SerializeField] private LayerMask objectLayer;

    private float holdTime = 0f;

    private bool isHolding = false;

    private float rightArea;

    private void Update()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;
        }

        if (inputReader.LeftTriggerIsPressed)
        {
            Collider[] cows = Physics.OverlapSphere(transform.position, radius, objectLayer);

            foreach (Collider cow in cows)
            {
                if (IsInCone(cow.transform.position))
                {
                    Debug.Log($"{cow.name}");
                    // On apply le state d'affraid à la vache
                }
            }
        }
    }

    private bool IsInCone(Vector3 position)
    {
        Vector3 directionToObj = (position - transform.position).normalized;
        return Vector3.Angle(transform.forward, directionToObj) < rightArea / 2;
    }

    public override void UseTool(bool isHeld)
    {
        if (isHeld)
        {
            isHolding = true;
            Debug.Log("yes");
        }
        else
        {
            isHolding = false;

            rightArea = holdTime >= holdThreshold ? bigDisturbAreaRange : littleDisturbAreaRange;

            holdTime = 0f;
        }
    }
}