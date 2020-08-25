using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Generic Food Item", menuName ="InventoryData/FoodItemSO")]
public class FoodItemSO : ItemSO
{
    public int staminaBonus = 0, hungerBonus = 0, energyBonus = 0;

    public override bool IsUsable()
    {
        return true;
    }

}
