using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Generic Weapon Item", menuName = "InventoryData/WeaponItemSO")]
public class WeponItemSO : ItemSO
{
    public int damageMin = 0, damageMax = 0;

    [Range(0,1)]
    public float criticalChance = 0.2f;
}
