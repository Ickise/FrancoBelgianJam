using UnityEngine;

public class AirHorn : ToolBase
{
    [SerializeField, Header("References")] private GameObject littleDisturbPrefab;
    [SerializeField] private GameObject bigDisturbPrefab;

    [SerializeField] private Animator animator;

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
            animator.SetTrigger("Scaring");

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
            animator.SetTrigger("Default");
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
        Vector3 direction = transform.forward;

        int numberOfRays = 30;
        float coneAngle = currentAreaRange / 2f;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angleOffset = Random.Range(-coneAngle, coneAngle);
            Vector3 rayDirection = Quaternion.Euler(0, angleOffset, 0) * direction;

            Ray ray = new Ray(transform.position, rayDirection);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, radius, objectLayer))
            {
                CowController cow = hit.collider.GetComponent<CowController>();
                if (cow != null)
                {
                    if (isBigDisturb && !cow.HasBeenFarted())
                    {
                        cow.SwitchState(new FartState(), transform.position);
                        cow.SetFarted(true);
                    }
                    else if (!isBigDisturb && !cow.HasBeenScared())
                    {
                        cow.SwitchState(new AfraidState(), transform.position);
                        cow.SetScared(true);
                    }
                }
            }
        }
    }

    public override void UseTool(bool isHeld)
    {
    }
}