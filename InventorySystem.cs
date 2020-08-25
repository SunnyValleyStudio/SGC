using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using System;
using SVS.InventorySystem;

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
        ItemData artificialItem = new ItemData(0, 10, "7dd5920bb8ee4839a6bb006750c1657e", true, 100);
        AddToStorage(artificialItem);
        var hotbarUiElementsList = uiInventory.GetUiElementsForHotbar();

        for (int i = 0; i < hotbarUiElementsList.Count; i++)
        {
            inventoryData.AddHotbarUiElement(hotbarUiElementsList[i].GetInstanceID());
            hotbarUiElementsList[i].OnClickEvent += UseHotbarItemHandler;
        }
    }

    private void UseHotbarItemHandler(int ui_id, bool isEmpty)
    {
        Debug.Log("Using hotbar item");
        if (isEmpty)
            return;
        //throw new NotImplementedException();
        
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
        var uiElementsList = uiInventory.GetUiElementsForInventory();
        var inventoryItemsList = inventoryData.GetItemsDataForInventory();
        for (int i = 0; i < uiElementsList.Count; i++)
        {
            var uiItemElement = uiElementsList[i];
            var itemData = inventoryItemsList[i];
            if (itemData.IsNull == false)
            {
                var itemName = ItemDataManager.instance.GetItemName(itemData.ID);
                var itemSprite = ItemDataManager.instance.GetItemSprite(itemData.ID);
                uiItemElement.SetInventoryUiElement(itemName, itemData.Count, itemSprite);
            }
            inventoryData.AddInventoryUiElement(uiItemElement.GetInstanceID());
        }
    }

    private void PrepareUI()
    {
        uiInventory.PrepareInventoryItems(inventoryData.PlayerStorageLimit);
        AddEventHandlersToInventoryUiElements();
    }

    private void AddEventHandlersToInventoryUiElements()
    {
        foreach (var uiItemElement in uiInventory.GetUiElementsForInventory())
        {
            uiItemElement.OnClickEvent += UiElementSelectedHandler;
        }
    }

    private void UiElementSelectedHandler(int ui_id, bool isEmpty)
    {
        Debug.Log("Selecting inventory item");
        if (isEmpty)
            return;
        inventoryData.ResetSelectedItem();
        inventoryData.SetSelectedItemTo(ui_id);
        
    }

    public int AddToStorage(IInventoryItem item)
    {
        int val = inventoryData.AddToStorage(item);
        return val;
    }
}
