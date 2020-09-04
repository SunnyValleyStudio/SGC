using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using System;
using SVS.InventorySystem;
using UnityEngine.EventSystems;

public class InventorySystem : MonoBehaviour, ISavable
{
    public Action onInventoryStateChanged, OnStructureUse;

    private UiInventory uiInventory;

    private InventorySystemData inventoryData;

    public int playerStorageSize = 20;

    public InteractionManager interactionManager;

    public bool WeaponEquipped { get => inventoryData.ItemEquipped;}
    public string EquippedWeaponId { get => inventoryData.EquippedItemId; }
    public bool InventoryVisible { get => uiInventory.IsInventoryVisible; }

    public StructureItemSO selectedStructureData = null;
    public int selectedStructureUiId = 0;

    private void Awake()
    {
        uiInventory = GetComponent<UiInventory>(); 
    }

    private void Start()
    {
        inventoryData = new InventorySystemData(playerStorageSize, uiInventory.HotbarElementsCount);
        inventoryData.updateHotbarCallback += UpdateHotbarHandler;

        uiInventory.AssignDropButtonHandler(DropItemHandler);
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

    internal void RemoveSelectedStructureFromInventory()
    {
        RemoveItemFromInventory(selectedStructureUiId);
        selectedStructureUiId = 0;
        selectedStructureData = null;
    }

    private void RemoveItemFromInventory(int ui_id)
    {
        inventoryData.TakeOneFromItem(ui_id);
        if (inventoryData.CheckIfSelectedItemIsEmpty(ui_id))
        {
            ClearUIElement(ui_id);
            inventoryData.RemoveItemFromInventory(ui_id);
        }
        else
        {
            UpdateUI(ui_id, inventoryData.GetItemCountFor(ui_id));
        }
        onInventoryStateChanged.Invoke();
    }

    internal void CraftAnItem(RecipeSO recipe)
    {
        foreach (var recipeIngredient in recipe.ingredientsRequired)
        {
            inventoryData.TakeFromItem(recipeIngredient.ingredient.ID, recipeIngredient.count);
        }
        inventoryData.AddToStorage(recipe);
        UpdateInventoryItems();
        UpdateHotbarHandler();
        onInventoryStateChanged.Invoke();
    }

    private void UpdateInventoryItems()
    {
        ToggleInventory();
        ToggleInventory();
    }

    internal bool CheckInventoryFull()
    {
        return inventoryData.CheckIfStorageIsFull();
    }

    internal bool CheckResourceAvailability(string id, int count)
    {
        return inventoryData.CheckItemInStorage(id, count);
    }

    internal void HotbarShortKeyHandler(int hotbarKey)
    {
        var ui_index = hotbarKey == 0 ? 9 : hotbarKey - 1;
        var uiElementID = uiInventory.GetHotbarElementUiIDWithIndex(ui_index);
        if (uiElementID == -1)
        {
            return;
        }
        var id = inventoryData.GetItemIdFor(uiElementID);
        if (id == null)
            return;
        var itemData = ItemDataManager.instance.GetItemData(id);
        UseItem(itemData, uiElementID);

    }

    private void UseInventoryItemHandler()
    {
        var itemData = ItemDataManager.instance.GetItemData(inventoryData.GetItemIdFor(inventoryData.selectedItemUIID));
        UseItem(itemData, inventoryData.selectedItemUIID);
    }

    private void UseItem(ItemSO itemData, int ui_id)
    {
        if(itemData.GetItemType() == ItemType.Structure)
        {
            selectedStructureUiId = ui_id;
            selectedStructureData = (StructureItemSO)itemData;
            OnStructureUse.Invoke();
            return;
        }
        if (interactionManager.UseItem(itemData))
        {
            RemoveItemFromInventory(ui_id);
        }else if (interactionManager.EquipItem(itemData))
        {
            DeselectCurrentItem();
            ItemSpawnManager.instance.RemoveItemFromPlayerHand();
            if (inventoryData.ItemEquipped)
            {
                uiInventory.ToggleEquipSelectedItem(inventoryData.EquippedUI_ID);
                if (inventoryData.EquippedUI_ID == ui_id)
                {
                    inventoryData.UnequipItem();
                    return;
                }
            }
            inventoryData.EquipItem(ui_id);
            uiInventory.ToggleEquipSelectedItem(ui_id);
            ItemSpawnManager.instance.CreateItemObjectInPlayerHand(itemData.ID);
        }
        
    }

    private void UpdateUI(int ui_id, int count)
    {
        uiInventory.UpdateItemInfo(ui_id, count);
    }

    private void DropItemHandler()
    {
        var id = inventoryData.selectedItemUIID;
        ItemSpawnManager.instance.CreateItemAtPlayersFeet(inventoryData.GetItemIdFor(id), inventoryData.GetItemCountFor(id));
        ClearUIElement(id);
        inventoryData.RemoveItemFromInventory(id);
        onInventoryStateChanged.Invoke();
    }

    private void ClearUIElement(int ui_id)
    {
        uiInventory.DisableHighlightForSelectedItem(ui_id);
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
                uiItemElement.SetItemUI(itemName, itemData.Count, itemSprite);
            }
        }
    }

    private void UseHotbarItemHandler(int ui_id, bool isEmpty)
    {
        if (isEmpty)
            return;
        DeselectCurrentItem();
        var itemData = ItemDataManager.instance.GetItemData(inventoryData.GetItemIdFor(ui_id));
        UseItem(itemData, ui_id);
        
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
                uiItemElement.SetItemUI(itemName, itemData.Count, itemSprite);
            }
            inventoryData.AddInventoryUiElement(uiItemElement.GetInstanceID());
        }
        for (int i = 0; i < uiElementsList.Count; i++)
        {
            var uiItemElement = uiElementsList[i];
            if (inventoryData.EquippedUI_ID == uiItemElement.GetInstanceID())
            {
                uiItemElement.ToggleEquippedIndicator();
            }
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
        if (inventoryData.selectedItemUIID != -1)
        {
            uiInventory.DisableHighlightForSelectedItem(inventoryData.selectedItemUIID);
            uiInventory.ToggleItemButtons(false, false);
        }
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
        if (isEmpty)
            return;
        DeselectCurrentItem();
        inventoryData.SetSelectedItemTo(ui_id);
        uiInventory.HighlightSelectedItem(ui_id);
        uiInventory.ToggleItemButtons(ItemDataManager.instance.IsItemUsabel(inventoryData.GetItemIdFor(inventoryData.selectedItemUIID)),true);
        if (inventoryData.ItemEquipped)
        {
            if(ui_id == inventoryData.EquippedUI_ID)
            {
                uiInventory.ToggleItemButtons(ItemDataManager.instance.IsItemUsabel(inventoryData.GetItemIdFor(inventoryData.selectedItemUIID)), false);
            }
        }
    }

    public int AddToStorage(IInventoryItem item)
    {
        int val = inventoryData.AddToStorage(item);
        return val;
    }

    public string GetJsonDataToSave()
    {
        return JsonUtility.ToJson(inventoryData.GetDataToSave());
    }

    public void LoadJsonData(string jsonData)
    {
        SavedItemSystemData dataToLoad = JsonUtility.FromJson<SavedItemSystemData>(jsonData);
        inventoryData.LoadData(dataToLoad);
    }
}
