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
        ItemData artificialItem = new ItemData(0, 10, "7dd5920bb8ee4839a6bb006750c1657e", true, 100);
        AddToStorage(artificialItem);
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
                    Debug.Log("Swapping between inventory items");
                }
                else
                {
                    Debug.Log("Swapping between inventory -> hotbar");
                }
            }
            else
            {
                if (uiInventory.CheckItemInInventory(draggedItemID))
                {
                    Debug.Log("Swapping between hotbar -> inventory");
                }
                else
                {
                    Debug.Log("Swapping between hotbar items");
                }
            }
            
        }
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
