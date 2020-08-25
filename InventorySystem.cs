using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class InventorySystem : MonoBehaviour
{
    private UiInventory uiInventory;

    private InventorySystemData inventoryData;

    public int playerStorageSize = 20;

    private void Awake()
    {
        uiInventory = GetComponent<UiInventory>(); 
    }

    private void Start()
    {
        inventoryData = new InventorySystemData(playerStorageSize, uiInventory.HotbarElementsCount);
    }

    public void ToggleInventory()
    {
        if(uiInventory.IsInventoryVisible == false)
        {
            //populate inventory
        }
        uiInventory.ToggleUI();
    }
}
