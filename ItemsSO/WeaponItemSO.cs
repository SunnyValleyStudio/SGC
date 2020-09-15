using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Generic Weapon Item", menuName = "InventoryData/WeaponItemSO")]
public class WeaponItemSO : ItemSO
{
    public int damageMin = 0, damageMax = 0;

    [Range(0,1)]
    public float criticalChance = 0.2f;

    public override bool IsUsable()
    {
        return true;
    }

    internal int GetDamageValue()
    {
        var randomCriticalChange = UnityEngine.Random.value;
        if(randomCriticalChange < criticalChance)
        {
            return damageMax * 2;
        }
        return UnityEngine.Random.Range(damageMin, damageMax+1);
    }
}
