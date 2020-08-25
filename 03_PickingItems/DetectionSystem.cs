using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    private List<Collider> collidersList = new List<Collider>();
    private Collider currentCollider;

    public LayerMask objectDetectionMask;

    public float detectionRadius;

    public Collider CurrentCollider { get => currentCollider; set => currentCollider = value; }

    public Collider[] DetectObjectsInFront(Vector3 movementDirectionVector)
    {
        return Physics.OverlapSphere(transform.position + movementDirectionVector, detectionRadius, objectDetectionMask);
    }

    public void PerformDetection(Vector3 movementDirectionVector)
    {
        var colliders = DetectObjectsInFront(movementDirectionVector);
        collidersList.Clear();
        foreach (var collider in colliders)
        {
            var pickableItem = collider.GetComponent<IPickable>();
            if (pickableItem != null)
            {
                collidersList.Add(collider);
            }
        }
        if (collidersList.Count == 0)
        {
            if (currentCollider != null)
            {
                currentCollider = null;
            }
            return;
        }
        if (currentCollider == null)
        {
            currentCollider = collidersList[0];
        }else if (collidersList.Contains(currentCollider) == false)
        {
            currentCollider = collidersList[0];
        }
        Debug.Log(collidersList.Count);
    } 
}
