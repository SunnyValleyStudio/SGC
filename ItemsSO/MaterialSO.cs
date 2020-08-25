using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MaterialItemData", menuName ="InventoryData/MaterialSO")]
public class MaterialSO : ItemSO
{
    public ResourceType resourceType;
    private void OnEnable()
    {
        itemType = ItemType.Material;
    }
}

public enum ResourceType
{
    None,
    Wood,
    Stone
}
