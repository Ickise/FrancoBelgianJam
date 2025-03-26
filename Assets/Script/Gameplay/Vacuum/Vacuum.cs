using UnityEngine;
using System.Collections.Generic;

public class Vacuum : ToolBase
{
    [SerializeField, Header("Settings")] private float suctionRadius = 3f;
    [SerializeField] private float suctionAngle = 45f;
    [SerializeField] private float suctionPower = 5f;
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private int scorePerObject = 10;

    [SerializeField] private InputReader inputReader;

    private List<Rigidbody> suckedObjects = new List<Rigidbody>();

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
                    if (objRb != null)
                    {
                        objRb.linearVelocity = (transform.position - obj.transform.position).normalized * suctionPower;
                        if (!suckedObjects.Contains(objRb))
                        {
                            suckedObjects.Add(objRb);
                        }
                    }

                    if (Vector3.Distance(transform.position, obj.transform.position) < 1f)
                    {
                        suckedObjects.Remove(objRb);
                        Destroy(obj.gameObject);
                        ScoreManager.instance.ChangeScoreValue(scorePerObject, true);
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