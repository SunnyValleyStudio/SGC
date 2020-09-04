using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialHelper
{
    public static void SwapToSelectionMaterial(GameObject objectToModify, List<Material[]> currentColliderMaterialsList, Material selectionMaterial)
    {
        currentColliderMaterialsList.Clear();
        PrepareRendererToSwapMaterials(objectToModify, currentColliderMaterialsList, selectionMaterial);
        if (objectToModify.transform.childCount > 0)
        {
            foreach (Transform child in objectToModify.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    PrepareRendererToSwapMaterials(child.gameObject, currentColliderMaterialsList, selectionMaterial);
                }
            }
        }
    }

    public static void PrepareRendererToSwapMaterials(GameObject objectToModify, List<Material[]> currentColliderMaterialsList, Material selectionMaterial)
    {
        var renderer = objectToModify.GetComponent<Renderer>();
        currentColliderMaterialsList.Add(renderer.sharedMaterials);
        SwapMaterials(renderer, selectionMaterial);
    }

    public static void SwapMaterials(Renderer renderer, Material selectionMaterial)
    {
        Material[] matArray = new Material[renderer.materials.Length];
        for (int i = 0; i < matArray.Length; i++)
        {
            matArray[i] = selectionMaterial;
        }
        renderer.materials = matArray;
    }

    public static void SwapToOriginalMaterial(GameObject objectToModify, List<Material[]> currentColliderMaterialsList)
    {
        var renderer = objectToModify.GetComponent<Renderer>();
        renderer.materials = currentColliderMaterialsList[0];
        if (currentColliderMaterialsList.Count > 1)
        {
            for (int i = 0; i < currentColliderMaterialsList.Count; i++)
            {
                if (objectToModify.transform.GetChild(i).gameObject.activeSelf)
                {
                    var childRenderer = objectToModify.transform.GetChild(i).GetComponent<Renderer>();
                    childRenderer.materials = currentColliderMaterialsList[i];
                }
            }
        }
    }
}
