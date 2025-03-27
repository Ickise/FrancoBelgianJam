using UnityEngine;

public class AirHorn : ToolBase
{
    [SerializeField, Header("References")] private GameObject littleDisturb;
    [SerializeField] private GameObject bigDisturb;

    [SerializeField] private Animator animator;
    [SerializeField] private InputReader inputReader;

    [SerializeField, Header("Settings")] private float holdThreshold = 3f;
    [SerializeField] private float littleDisturbAreaRange = 60f;
    [SerializeField] private float bigDisturbAreaRange = 90f;
    [SerializeField] private float radius = 3f;

    [SerializeField] private LayerMask objectLayer;

    private float holdTime = 0f;
    private float currentAreaRange;
    private bool isBigDisturb = false;

    private void Update()
    {
        if (inputReader.LeftTriggerIsPressed)
        {
            holdTime += Time.deltaTime;
            animator.SetTrigger("Scaring");

            if (!littleDisturb.activeSelf && !bigDisturb.activeSelf)
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

        if (littleDisturb.activeSelf || bigDisturb.activeSelf)
        {
            DetectObjectsInCone();
        }
    }

    private void StartDisturb()
    {
        isBigDisturb = false;
        currentAreaRange = littleDisturbAreaRange;
        littleDisturb.SetActive(true);
        bigDisturb.SetActive(false);
    }

    private void TransformToBigDisturb()
    {
        isBigDisturb = true;
        currentAreaRange = bigDisturbAreaRange;
        littleDisturb.SetActive(false);
        bigDisturb.SetActive(true);

        //nvoke(nameof(StopDisturb), bigDisturbAreaTimeLife);
    }

    private void StopDisturb()
    {
        littleDisturb.SetActive(false);
        bigDisturb.SetActive(false);
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
            if (Physics.Raycast(ray, out RaycastHit hit, radius, objectLayer))
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
