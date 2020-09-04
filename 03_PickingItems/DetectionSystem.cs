using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    public Action<Collider, Vector3> OnAttackSuccessful;
    private List<Collider> collidersList = new List<Collider>();
    private Collider currentCollider;
    private List<Material[]> currentColliderMaterialsList = new List<Material[]>();

    public LayerMask objectDetectionMask;

    public Material selectionMaterial;

    public float detectionRadius;

    public Collider CurrentCollider { get => currentCollider; set => currentCollider = value; }

    public Transform WeaponRaycastStartPosition;

    public float attackDistance = 0.8f;

    private Collider iUsableCollider;

    public Collider IUsableCollider
    {
        get { return iUsableCollider; }
        private set { iUsableCollider = value; }
    }


    public Collider[] DetectObjectsInFront(Vector3 movementDirectionVector)
    {
        return Physics.OverlapSphere(transform.position + movementDirectionVector, detectionRadius, objectDetectionMask);
    }

    public void PerformDetection(Vector3 movementDirectionVector)
    {
        var colliders = DetectObjectsInFront(movementDirectionVector);
        collidersList.Clear();
        bool isUsableFound = false;
        foreach (var collider in colliders)
        {
            var pickableItem = collider.GetComponent<IPickable>();
            if (pickableItem != null)
            {
                collidersList.Add(collider);
            }
            var usable = collider.GetComponent<IUsable>();
            if(usable != null && isUsableFound == false)
            {
                IUsableCollider = collider;
                isUsableFound = true;
            }
        }
        if(isUsableFound == false)
        {
            IUsableCollider = null;
        }
        if (collidersList.Count == 0)
        {
            if (currentCollider != null)
            {
                MaterialHelper.SwapToOriginalMaterial(currentCollider.gameObject, currentColliderMaterialsList);
                currentCollider = null;
            }
            return;
        }
        if (currentCollider == null)
        {
            currentCollider = collidersList[0];
            MaterialHelper.SwapToSelectionMaterial(currentCollider.gameObject, currentColliderMaterialsList, selectionMaterial);
        }
        else if (collidersList.Contains(currentCollider) == false)
        {
            MaterialHelper.SwapToOriginalMaterial(currentCollider.gameObject, currentColliderMaterialsList);
            currentCollider = collidersList[0];
            MaterialHelper.SwapToSelectionMaterial(currentCollider.gameObject,currentColliderMaterialsList,selectionMaterial);
        }
    } 

    

    public void DetectColliderInFront()
    {
        RaycastHit hit;
        if(Physics.SphereCast(WeaponRaycastStartPosition.position,0.2f, transform.forward, out hit, attackDistance))
        {
            OnAttackSuccessful?.Invoke(hit.collider, hit.point);
        }

    }
}
