using SVS.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    IInventoryItem PickUp();

    void SetCount(int value);
}
