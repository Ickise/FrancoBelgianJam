using UnityEngine;

public class Vacuum : ToolBase
{
    [SerializeField, Header("Settings")] private float suctionRadius = 3f;
    [SerializeField] private LayerMask objectLayer;

    public override void UseTool(bool isHeld)
    {
        Collider[] objectsToSuck = Physics.OverlapSphere(transform.position, suctionRadius, objectLayer);

        foreach (Collider obj in objectsToSuck)
        {
            Rigidbody objRb = obj.GetComponent<Rigidbody>();
            if (objRb != null)
            {
                objRb.linearVelocity = (transform.position - obj.transform.position).normalized * 5f;
            }
        }
    }
}