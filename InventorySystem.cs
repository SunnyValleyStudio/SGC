using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using System;
using SVS.InventorySystem;
using UnityEngine.EventSystems;

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
        inventoryData.updateHotbarCallback += UpdateHotbarHandler;

        uiInventory.AssignDropButtonHandler(DropHandler);
        uiInventory.AssignUseButtonHandler(UseInventoryItemHandler);

        //ItemData artificialItem = new ItemData(0, 20, "7dd5920bb8ee4839a6bb006750c1657e", true, 100);
        //ItemData artificialItem1 = new ItemData(0, 90, "7dd5920bb8ee4839a6bb006750c1657e", true, 100);
        //AddToStorage(artificialItem);
        //AddToStorage(artificialItem1);
        var hotbarUiElementsList = uiInventory.GetUiElementsForHotbar();

        for (int i = 0; i < hotbarUiElementsList.Count; i++)
        {
            inventoryData.AddHotbarUiElement(hotbarUiElementsList[i].GetInstanceID());
            hotbarUiElementsList[i].OnClickEvent += UseHotbarItemHandler;
            hotbarUiElementsList[i].DragContinueCallback += DraggingHandler;
            hotbarUiElementsList[i].DragStartCallback += DragStartHandler;
            hotbarUiElementsList[i].DragStopCallback += DragStopHandler;
            hotbarUiElementsList[i].DropCalback += DropHandler;
        }
    }

    private void UseInventoryItemHandler()
    {
        Debug.Log("Using item");
    }

    private void DropHandler()
    {
        ClearUIElement(inventoryData.selectedItemUIID);
        inventoryData.RemoveItemFromInventory(inventoryData.selectedItemUIID);
    }

    private void ClearUIElement(int ui_id)
    {
        uiInventory.ClearItemElement(ui_id);
        uiInventory.ToggleItemButtons(false, false);
    }

    private void UpdateHotbarHandler()
    {
        var uiElementsList = uiInventory.GetUiElementsForHotbar();
        var hotbarItemsList = inventoryData.GetItemsDataForHotbar();
        for (int i = 0; i < uiElementsList.Count; i++)
        {
            var uiItemElement = uiElementsList[i];
            uiItemElement.ClearItem();
            var itemData = hotbarItemsList[i];
            if (itemData.IsNull == false)
            {
                var itemName = ItemDataManager.instance.GetItemName(itemData.ID);
                var itemSprite = ItemDataManager.instance.GetItemSprite(itemData.ID);
                uiItemElement.SetInventoryUiElement(itemName, itemData.Count, itemSprite);
            }
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
            uiItemElement.DragContinueCallback += DraggingHandler;
            uiItemElement.DragStartCallback += DragStartHandler;
            uiItemElement.DragStopCallback += DragStopHandler;
            uiItemElement.DropCalback += DropHandler;
        }
    }

    private void DropHandler(PointerEventData eventData, int droppedItemID)
    {
        if(uiInventory.Draggableitem != null)
        {
            var draggedItemID = uiInventory.DraggableItemPanel.GetInstanceID();
            if (draggedItemID == droppedItemID)
            {
                return;
            }
            DeselectCurrentItem();
            if (uiInventory.CheckItemInInventory(draggedItemID))

            {
                if (uiInventory.CheckItemInInventory(droppedItemID))
                {
                    DroppingItemsInventoryToInventory(droppedItemID, draggedItemID);
                }
                else
                {
                    DroppingItemsInventoryToHotbar(droppedItemID, draggedItemID);
                }
            }
            else
            {
                if (uiInventory.CheckItemInInventory(droppedItemID))
                {
                    DroppingItemsHotbarToInventory(droppedItemID, draggedItemID);
                }
                else
                {
                    DroppingItemsHotbarToHotbar(droppedItemID, draggedItemID);
                }
            }
            
        }
    }

    private void DroppingItemsHotbarToHotbar(int droppedItemID, int draggedItemID)
    {
        uiInventory.SwapUiItemHotbarToHotbar(droppedItemID, draggedItemID);
        inventoryData.SwapStorageItemsInsideHotbar(droppedItemID, draggedItemID);
    }

    private void DroppingItemsHotbarToInventory(int droppedItemID, int draggedItemID)
    {
        uiInventory.SwapUiItemHotbarToInventory(droppedItemID, draggedItemID);
        inventoryData.SwapStorageHotbarToInventory(droppedItemID, draggedItemID);
    }

    private void DroppingItemsInventoryToHotbar(int droppedItemID, int draggedItemID)
    {
        uiInventory.SwapUiItemInventoryToHotbar(droppedItemID, draggedItemID);
        inventoryData.SwapStorageInventoryToHotbar(droppedItemID, draggedItemID);
    }

    private void DroppingItemsInventoryToInventory(int droppedItemID, int draggedItemID)
    {
        uiInventory.SwapUiItemInventoryToInventory(droppedItemID, draggedItemID);
        inventoryData.SwapStorageItemsInsideInventory(droppedItemID, draggedItemID);
    }

    private void DeselectCurrentItem()
    {
        inventoryData.ResetSelectedItem();
    }

    private void DragStopHandler(PointerEventData eventData)
    {
        uiInventory.DestroyDraggedObject();
    }

    private void DragStartHandler(PointerEventData eventData, int ui_id)
    {
        uiInventory.DestroyDraggedObject();
        uiInventory.CreateDraggableItem(ui_id);
    }

    private void DraggingHandler(PointerEventData eventData)
    {
        uiInventory.MoveDraggableItem(eventData);
    }

    private void UiElementSelectedHandler(int ui_id, bool isEmpty)
    {
        Debug.Log("Selecting inventory item");
        if (isEmpty)
            return;
        DeselectCurrentItem();
        inventoryData.SetSelectedItemTo(ui_id);
        
    }

    public int AddToStorage(IInventoryItem item)
    {
        int val = inventoryData.AddToStorage(item);
        return val;
    }
}
