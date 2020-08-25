using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private UiInventory uiInventory;

    private void Awake()
    {
        uiInventory = GetComponent<UiInventory>();    
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
