using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="StructureItemData", menuName ="InventoryData/StructureItemSO")]
public class StructureItemSO : ItemSO
{
    private void OnEnable()
    {
        itemType = ItemType.Structure;
    }

    public override bool IsUsable()
    {
        return true;
    }
}
