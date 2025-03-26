using UnityEngine;
using System.Collections.Generic;

public class Vacuum : ToolBase
{
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

    private void Start()
    {
        gasManager = GasManager.instance;
        scoreManager = ScoreManager.instance;
    }

    private void Update()
    {
        if (inputReader.RightTriggerIsPressed)
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

                        fart.SwitchState(new FleeState(fart));
                        if (!suckedObjects.Contains(objRb))
                        {
                            suckedObjects.Add(objRb);
                        }
                    }

                    if (Vector3.Distance(transform.position, obj.transform.position) < .5f)
                    {
                        fart.SwitchState(new CatchState(fart));
                        suckedObjects.Remove(objRb);
                        Destroy(obj.gameObject);
                        gasManager.ChangeGasStockValue(gasNumber, true);
                        scoreManager.ChangeScoreValue(scorePerObject, true);
                    }
                }
                else
                {
                    StopSuction();
                }
            }
        }
        else
        {
            StopSuction();
        }
    }

    private bool IsInSuctionCone(Vector3 position)
    {
        Vector3 directionToObj = (position - transform.position).normalized;
        return Vector3.Angle(transform.forward, directionToObj) < suctionAngle / 2;
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