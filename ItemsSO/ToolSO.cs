using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ToolItemData", menuName ="InventoryData/ToolSO")]
public class ToolSO : WeaponItemSO
{
    public int basicHarvestPower = 3;
    public ResourceType resourceBoosted;
    public int boostedHarvestPower = 6;

    private void OnEnable()
    {
        itemType = ItemType.Weapon;
    }

    public int GetResourceHarvested(ResourceType resourceType)
    {
        if (resourceBoosted == resourceType)
        {
            return boostedHarvestPower;
        }
        return basicHarvestPower;
    }

}


