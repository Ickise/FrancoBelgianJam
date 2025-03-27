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
    [SerializeField] private float bigDisturbAreaDistance = 5f;
    [SerializeField] private float bigDisturbAreaTimeLife = 2f;
    [SerializeField] private float radius = 3f;

    [SerializeField] private LayerMask objectLayer;

    private float holdTime = 0f;
    private GameObject currentDisturbArea;
    private float currentAreaRange;
    private bool isBigDisturb = false;

    private void Update()
    {
        if (inputReader.LeftTriggerIsPressed)
        {
            holdTime += Time.deltaTime;

            if (currentDisturbArea == null)
            {
                StartDisturb();
            }

            if (!isBigDisturb && holdTime >= holdThreshold)
            {
                TransformToBigDisturb();
            }
        }
        else
        {
            StopDisturb();
        }

        if (currentDisturbArea != null)
        {
            DetectObjectsInCone();
        }
    }

    private void StartDisturb()
    {
        isBigDisturb = false;
        currentAreaRange = littleDisturbAreaRange;
        currentDisturbArea = Instantiate(littleDisturbPrefab, transform.position, Quaternion.identity);
    }

    private void TransformToBigDisturb()
    {
        if (currentDisturbArea != null)
        {
            Destroy(currentDisturbArea);
        }

        isBigDisturb = true;
        currentAreaRange = bigDisturbAreaRange;
        currentDisturbArea = Instantiate(bigDisturbPrefab, transform.position, Quaternion.identity);

        Destroy(currentDisturbArea, bigDisturbAreaTimeLife);
    }

    private void StopDisturb()
    {
        if (!isBigDisturb && currentDisturbArea != null)
        {
            Destroy(currentDisturbArea);
        }

        currentDisturbArea = null;
        holdTime = 0f;
    }

    private void DetectObjectsInCone()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, radius, objectLayer);

        foreach (Collider obj in objects)
        {
            if (IsInCone(obj.transform.position))
            {
                CowController cow = obj.GetComponent<CowController>();
                if (cow != null)
                {
                    if (isBigDisturb && !cow.HasBeenFarted())
                    {
                        cow.SwitchState(new FartState(), transform.position);
                        cow.SetFarted(true);
                    }
                    else if (!isBigDisturb && !cow.HasBeenScared())
                    {
                        {
                            cow.SwitchState(new AfraidState(), transform.position);
                            cow.SetScared(true);
                        }
                    }
                }
            }
        }
    }

    private bool IsInCone(Vector3 position)
    {
        Vector3 directionToObj = (position - transform.position).normalized;
        return Vector3.Angle(transform.forward, directionToObj) < currentAreaRange / 2;
    }

    public override void UseTool(bool isHeld)
    {
    }
}