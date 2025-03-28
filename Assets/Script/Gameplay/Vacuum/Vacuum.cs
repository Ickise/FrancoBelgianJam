using UnityEngine;
using System.Collections.Generic;

public class Vacuum : ToolBase
{
    [SerializeField, Header("References")] private Animator animator;
    [SerializeField] private GameObject vacuumEffect;
    [SerializeField] private ParticleSystem suctionParticles;

    [SerializeField, Header("Settings")] private float suctionRadius = 0.2f;
    [SerializeField] private float suctionAngle = 150f;
    [SerializeField] private float suctionLength = 2f;
    [SerializeField] private float suctionPower = 5f;
    [SerializeField] private float gasNumber = 1f;
    [SerializeField] private int scorePerObject = 10;
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private InputReader inputReader;

    private List<Rigidbody> suckedObjects = new List<Rigidbody>();

    private GasManager gasManager;
    private ScoreManager scoreManager;
    private ParticleSystem.ShapeModule shapeModule;

    private void Start()
    {
        gasManager = GasManager.instance;
        scoreManager = ScoreManager.instance;

        if (suctionParticles != null)
        {
            shapeModule = suctionParticles.shape;
        }
    }

    private void Update()
    {
        if (inputReader.RightTriggerIsPressed)
        {
            animator.SetTrigger("Sucking");
            vacuumEffect.SetActive(true);
            //UpdateParticleSystem();
            DetectObjectsInCone();
        }
        else
        {
            animator.SetTrigger("Default");
            vacuumEffect.SetActive(false);
            StopSuction();
        }
    }

    private void UpdateParticleSystem()
    {
        if (suctionParticles != null)
        {
            shapeModule.angle = suctionAngle;
            shapeModule.radius = suctionRadius;
            shapeModule.scale = new Vector3(1, 1, suctionLength);
        }
    }

    private void DetectObjectsInCone()
    {
        Vector3 coneStart = transform.position;
        Vector3 coneDirection =
            Quaternion.Euler(0, 90, 0) * transform.forward; // Ici pour mettre le cône dans le bon sens
        Vector3 coneEnd = coneStart + coneDirection * suctionLength;

        Collider[] objectsToSuck = Physics.OverlapCapsule(coneStart, coneEnd, suctionRadius, objectLayer);

        foreach (Collider obj in objectsToSuck)
        {
            if (IsInSuctionCone(obj.transform.position, coneDirection))
            {
                Rigidbody objRb = obj.GetComponent<Rigidbody>();
                var fart = obj.GetComponent<FartController>();

                if (objRb != null)
                {
                    objRb.linearVelocity = (transform.position - obj.transform.position).normalized * suctionPower;

                    if (fart != null)
                    {
                        fart.SwitchState(new FleeState(fart));

                        if (!suckedObjects.Contains(objRb))
                        {
                            suckedObjects.Add(objRb);
                        }
                    }
                }

                if (Vector3.Distance(transform.position, obj.transform.position) < 1f)
                {
                    fart.SwitchState(new CatchState(fart));
                    suckedObjects.Remove(objRb);
                    Destroy(obj.gameObject);
                    gasManager.ChangeGasStockValue(gasNumber, true);
                    scoreManager.ChangeScoreValue(scorePerObject, true);
                }
            }
        }
    }

    private bool IsInSuctionCone(Vector3 position, Vector3 coneDirection)
    {
        Vector3 directionToObj = (position - transform.position).normalized;
        float angle = Vector3.Angle(coneDirection, directionToObj);
        return angle < suctionAngle / 2f;
    }

    public override void UseTool(bool isHeld)
    {
    }

    private void StopSuction()
    {
        foreach (Rigidbody objRb in suckedObjects)
        {
            if (objRb != null)
            {
                objRb.linearVelocity = Vector3.zero;
            }
        }

        suckedObjects.Clear();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.green;

        Vector3 coneStart = transform.position;
        Vector3 coneDirection = Quaternion.Euler(0, 90, 0) * transform.forward;

        float stepAngle = suctionAngle / 5f;

        for (float angle = -suctionAngle / 2f; angle <= suctionAngle / 2f; angle += stepAngle)
        {
            Vector3 dir = Quaternion.Euler(0, angle, 0) * coneDirection;
            Vector3 end = coneStart + dir * suctionLength;
            Gizmos.DrawLine(coneStart, end);
        }
    }
}