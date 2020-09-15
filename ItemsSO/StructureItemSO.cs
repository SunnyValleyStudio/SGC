using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="StructureItemData", menuName ="InventoryData/StructureItemSO")]
public class StructureItemSO : ItemSO
{
    [SerializeField]
    protected GameObject prefab;

    private void OnEnable()
    {
        itemType = ItemType.Structure;
    }

    public override bool IsUsable()
    {
        return true;
    }

    public override GameObject GetPrefab()
    {
        return prefab == null ? model : prefab;
    }
}
