using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MaterialItemData", menuName ="InventoryData/MaterialSO")]
public class MaterialSO : ItemSO
{
    private void OnEnable()
    {
        itemType = ItemType.Material;
    }
}
