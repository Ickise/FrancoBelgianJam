using UnityEngine;
using System.Collections.Generic;

public class Vacuum : ToolBase
{
    [SerializeField, Header("References")] private Animator animator;
    [SerializeField] private GameObject vacuumEffect;
    
    [SerializeField, Header("Settings")] private float suctionRadius = 3f;
    [SerializeField] private float suctionAngle = 45f;
    [SerializeField] private float suctionPower = 5f;
    [SerializeField] private float gasNumber = 1f;
    [SerializeField] private int scorePerObject = 10;
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private InputReader inputReader;
    
    private List<Rigidbody> suckedObjects = new List<Rigidbody>();

    private GasManager gasManager;
    private ScoreManager scoreManager;
    //private var _angleEffect;

    private void Start()
    {
        gasManager = GasManager.instance;
        scoreManager = ScoreManager.instance;
        var _angleEffect = vacuumEffect.GetComponent<ParticleSystem>().shape;
        _angleEffect.angle = suctionAngle; //change this value to upgrade the angle
        _angleEffect.length = suctionRadius;
    }

    private void Update()
    {
        if (inputReader.RightTriggerIsPressed)
        {
            animator.SetTrigger("Sucking");
            vacuumEffect.SetActive(true);
            DetectObjectsInCone();
        }
        else
        {
            animator.SetTrigger("Default");
            vacuumEffect.SetActive(false);
            StopSuction();
        }
    }

    private void DetectObjectsInCone()
    {
        Collider[] objectsToSuck = Physics.OverlapSphere(transform.position, suctionRadius, objectLayer);

        foreach (Collider obj in objectsToSuck)
        {
            if (IsInSuctionCone(obj.transform.position))
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

    private bool IsInSuctionCone(Vector3 position)
    {
        Vector3 directionToObj = (position - transform.position).normalized;

        float angle = Vector3.Angle(transform.forward, directionToObj);

        return angle < _angleEffect / 2f;
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
}