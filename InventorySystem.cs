using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using System;

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

        var hotbarUiElementsList = uiInventory.GetUiElementsForHotbar();

        for (int i = 0; i < hotbarUiElementsList.Count; i++)
        {
            inventoryData.AddHotbarUiElement(hotbarUiElementsList[i].GetInstanceID());
            hotbarUiElementsList[i].OnClickEvent += UseHotbarItemHandler;
        }
    }

    private void UseHotbarItemHandler(int arg1, bool arg2)
    {
        throw new NotImplementedException();
    }

    public void ToggleInventory()
    {
        if(uiInventory.IsInventoryVisible == false)
        {
            inventoryData.ResetSelectedItem();
            inventoryData.ClearInventoryUIElements();
            PrepareUI();
            PutDataInUI();
        }
        uiInventory.ToggleUI();
    }

    private void PutDataInUI()
    {
        throw new NotImplementedException();
    }

    private void PrepareUI()
    {
        throw new NotImplementedException();
    }
}
