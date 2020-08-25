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
                SwapToOriginalMaterial();
                currentCollider = null;
            }
            return;
        }
        if (currentCollider == null)
        {
            currentCollider = collidersList[0];
            SwapToSelectionMaterial();
        }else if (collidersList.Contains(currentCollider) == false)
        {
            SwapToOriginalMaterial();
            currentCollider = collidersList[0];
            SwapToSelectionMaterial();
        }
    } 

    private void SwapToSelectionMaterial()
    {
        currentColliderMaterialsList.Clear();
        if (currentCollider.transform.childCount > 0)
        {
            foreach (Transform child in currentCollider.transform)
            {
                PrepareRendererToSwapMaterials();
            }
        }
        else
        {
            PrepareRendererToSwapMaterials();

        }
    }

    private void PrepareRendererToSwapMaterials()
    {
        var renderer = currentCollider.GetComponent<Renderer>();
        currentColliderMaterialsList.Add(renderer.sharedMaterials);
        SwapMaterials(renderer);
    }

    private void SwapMaterials(Renderer renderer)
    {
        Material[] matArray = new Material[renderer.materials.Length];
        for (int i = 0; i < matArray.Length; i++)
        {
            matArray[i] = selectionMaterial;
        }
        renderer.materials = matArray;
    }

    private void SwapToOriginalMaterial()
    {
        if(currentColliderMaterialsList.Count > 1)
        {
            for (int i = 0; i < currentColliderMaterialsList.Count; i++)
            {
                var renderer = currentCollider.transform.GetChild(i).GetComponent<Renderer>();
                renderer.materials = currentColliderMaterialsList[i];
            }
        }
        else
        {
            var renderer = currentCollider.GetComponent<Renderer>();
            renderer.materials = currentColliderMaterialsList[0];
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
