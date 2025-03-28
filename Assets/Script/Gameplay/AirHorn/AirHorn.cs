using UnityEngine;

public class AirHorn : ToolBase
{
    [SerializeField, Header("References")] private GameObject littleDisturb;
    [SerializeField] private GameObject bigDisturb;

    [SerializeField] private ParticleSystem littleDisturbParticles;
    [SerializeField] private ParticleSystem bigDisturbParticles;

    [SerializeField] private Animator animator;
    [SerializeField] private InputReader inputReader;

    [SerializeField, Header("Settings")] private float holdThreshold = 3f;
    [SerializeField] private float radius = 2f;
    [SerializeField] private float coneLength = 3f;
    [SerializeField] private LayerMask objectLayer;

    private float holdTime = 0f;
    private bool isBigDisturb = false;
    private ParticleSystem.ShapeModule littleShape;
    private ParticleSystem.ShapeModule bigShape;

    private void Start()
    {
        if (littleDisturbParticles != null)
            littleShape = littleDisturbParticles.shape;

        if (bigDisturbParticles != null)
            bigShape = bigDisturbParticles.shape;
    }

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
        littleDisturb.SetActive(true);
        bigDisturb.SetActive(false);
    }

    private void TransformToBigDisturb()
    {
        isBigDisturb = true;
        littleDisturb.SetActive(false);
        bigDisturb.SetActive(true);
    }

    private void StopDisturb()
    {
        littleDisturb.SetActive(false);
        bigDisturb.SetActive(false);
        holdTime = 0f;
    }

    private void DetectObjectsInCone()
    {
        Vector3 coneStart = transform.position;
        Vector3 coneDirection = Quaternion.Euler(90, 0, 0) * transform.forward;
        Vector3 coneEnd = coneStart + coneDirection * coneLength;
        Collider[] objectsInCone = Physics.OverlapCapsule(coneStart, coneEnd, radius, objectLayer);

        foreach (Collider obj in objectsInCone)
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
                    cow.SwitchState(new AfraidState(), transform.position);
                    cow.SetScared(true);
                }
            }
        }
    }

    public override void UseTool(bool isHeld)
    {
    }
}