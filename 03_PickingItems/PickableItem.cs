using SVS.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickable, IInventoryItem
{
    [SerializeField]
    public ItemSO dataSource;
    public int count;

    public string ID => dataSource.ID;

    public int Count { 
        get 
        {
            if (dataSource.isStackable == false)
            {
                return 1;
            }
            return count;
        } 
    }

    public bool IsStackable => dataSource.isStackable;

    public int StackLimit => dataSource.stackLimit;

    public IInventoryItem PickUp()
    {
        return this;
    }

    public void SetCount(int value)
    {
        count = value;
        if (count == 0)
        {
            Destroy(gameObject);
        }
    }
}
