using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour
{
    public GameObject inventoryGeneralPanel;

    public UIStorageButtonsHelper uiStorageButtonHelper;

    public bool IsInventoryVisible { get => inventoryGeneralPanel.activeSelf; }
    public int HotbarElementsCount { get =>hotbarUiItems.Count;}
    public RectTransform Draggableitem { get => draggableitem; }
    public InventoryItemPanelHelper DraggableItemPanel { get => draggableItemPanel; }

    public Dictionary<int, InventoryItemPanelHelper> inventoryUiItems = new Dictionary<int, InventoryItemPanelHelper>();
    public Dictionary<int, InventoryItemPanelHelper> hotbarUiItems = new Dictionary<int, InventoryItemPanelHelper>();

    private List<int> listOfHotbarElementsID = new List<int>();

    public List<InventoryItemPanelHelper> GetUiElementsForHotbar()
    {
        return hotbarUiItems.Values.ToList();
    }

    public GameObject hotbarPanel, storagePanel;

    public GameObject storagePrefab;

    private RectTransform draggableitem;
    private InventoryItemPanelHelper draggableItemPanel;

    public Canvas canvas;

    private void Awake()
    {
        inventoryGeneralPanel.SetActive(false);
        foreach (Transform child in hotbarPanel.transform)
        {
            InventoryItemPanelHelper helper = child.GetComponent<InventoryItemPanelHelper>();
            if (helper != null)
            {
                hotbarUiItems.Add(helper.GetInstanceID(), helper);
                helper.isHotbarItem = true;
            }
        }
        listOfHotbarElementsID = hotbarUiItems.Keys.ToList();
    }

    public void ToggleUI()
    {
        if(inventoryGeneralPanel.activeSelf == false)
        {
            inventoryGeneralPanel.SetActive(true);
        }
        else
        {
            inventoryGeneralPanel.SetActive(false);
            DestroyDraggedObject();
        }
        uiStorageButtonHelper.HideAllButons();
    }

    internal int GetHotbarElementUiIDWithIndex(int ui_index)
    {
        if(listOfHotbarElementsID.Count <= ui_index)
        {
            return -1;
        }
        return listOfHotbarElementsID[ui_index];
    }

    public void AssignUseButtonHandler(Action handler)
    {
        uiStorageButtonHelper.OnUseBtnClick += handler;
    }

    internal void ClearItemElement(int ui_id)
    {
        GetItemFromCorrectDictionary(ui_id).ClearItem();
    }

    private InventoryItemPanelHelper GetItemFromCorrectDictionary(int ui_id)
    {
        if (inventoryUiItems.ContainsKey(ui_id))
        {
            return inventoryUiItems[ui_id];
        }else if (hotbarUiItems.ContainsKey(ui_id))
        {
            return hotbarUiItems[ui_id];
        }
        return null;
    }

    internal void UpdateItemInfo(int ui_id, int count)
    {
        GetItemFromCorrectDictionary(ui_id).UpdateCount(count);
    }

    public void AssignDropButtonHandler(Action handler)
    {
        uiStorageButtonHelper.OnDropBtnClick += handler;
    }

    public void ToggleItemButtons(bool useBtn, bool dropButton)
    {
        uiStorageButtonHelper.ToggleDropButton(dropButton);
        uiStorageButtonHelper.ToggleUseButton(useBtn);
    }

    public void PrepareInventoryItems(int playerStorageLimit)
    {
        for (int i = 0; i < playerStorageLimit; i++)
        {
            foreach (Transform child in storagePanel.transform)
            {
                Destroy(child.gameObject);
            }
        }
        inventoryUiItems.Clear();
        for (int i = 0; i < playerStorageLimit; i++)
        {
            var element = Instantiate(storagePrefab, Vector3.zero, Quaternion.identity, storagePanel.transform);
            var itemHelper = element.GetComponent<InventoryItemPanelHelper>();
            inventoryUiItems.Add(itemHelper.GetInstanceID(), itemHelper);
        }
    }

    public List<InventoryItemPanelHelper> GetUiElementsForInventory()
    {
        return inventoryUiItems.Values.ToList();
    }

    internal void ToggleEquipSelectedItem(int itemID)
    {
        if (hotbarUiItems.ContainsKey(itemID))
        {
            hotbarUiItems[itemID].ToggleEquippedIndicator();
        }
        else
        {
            inventoryUiItems[itemID].ToggleEquippedIndicator();
        }
    }

    public void DestroyDraggedObject()
    {
        if(draggableitem != null)
        {
            Destroy(draggableitem.gameObject);
            draggableItemPanel = null;
            draggableitem = null;
        }
    }

    public void CreateDraggableItem(int ui_id)
    {
        if (CheckItemInInventory(ui_id))
        {
            draggableItemPanel = inventoryUiItems[ui_id];
        }
        else
        {
            draggableItemPanel = hotbarUiItems[ui_id];
        }

        Image itemImage = draggableItemPanel.itemImage;
        var imageObject = Instantiate(itemImage, itemImage.transform.position, Quaternion.identity, canvas.transform);
        imageObject.raycastTarget = false;
        imageObject.sprite = itemImage.sprite;

        draggableitem = imageObject.GetComponent<RectTransform>();
        draggableitem.sizeDelta = new Vector2(100, 100);

    }

    public bool CheckItemInInventory(int ui_id)
    {
        return inventoryUiItems.ContainsKey(ui_id);
    }


    internal void MoveDraggableItem(PointerEventData eventData)
    {
        var valueToAdd = eventData.delta / canvas.scaleFactor;
        draggableitem.anchoredPosition += valueToAdd;
    }

    internal void SwapUiItemInventoryToInventory(int droppedItemID, int draggedItemID)
    {
        var tempName = inventoryUiItems[draggedItemID].itemName;
        var tempCount = inventoryUiItems[draggedItemID].itemCount;
        var tempSprite = inventoryUiItems[draggedItemID].itemImage.sprite;
        var tempisEmpty = inventoryUiItems[draggedItemID].isEmpty;

        var droppedItemData = inventoryUiItems[droppedItemID];
        inventoryUiItems[draggedItemID].SwapWithData(droppedItemData.itemName, droppedItemData.itemCount, droppedItemData.itemImage.sprite, droppedItemData.isEmpty);

        inventoryUiItems[droppedItemID].SwapWithData(tempName, tempCount, tempSprite, tempisEmpty);

        DestroyDraggedObject();
    }

    internal void SwapUiItemHotbarToHotbar(int droppedItemID, int draggedItemID)
    {
        var tempName = hotbarUiItems[draggedItemID].itemName;
        var tempCount = hotbarUiItems[draggedItemID].itemCount;
        var tempSprite = hotbarUiItems[draggedItemID].itemImage.sprite;
        var tempisEmpty = hotbarUiItems[draggedItemID].isEmpty;

        var droppedItemData = hotbarUiItems[droppedItemID];
        hotbarUiItems[draggedItemID].SwapWithData(droppedItemData.itemName, droppedItemData.itemCount, droppedItemData.itemImage.sprite, droppedItemData.isEmpty);

        hotbarUiItems[droppedItemID].SwapWithData(tempName, tempCount, tempSprite, tempisEmpty);

        DestroyDraggedObject();
    }

    internal void SwapUiItemHotbarToInventory(int droppedItemID, int draggedItemID)
    {
        var tempName = hotbarUiItems[draggedItemID].itemName;
        var tempCount = hotbarUiItems[draggedItemID].itemCount;
        var tempSprite = hotbarUiItems[draggedItemID].itemImage.sprite;
        var tempisEmpty = hotbarUiItems[draggedItemID].isEmpty;

        var droppedItemData = inventoryUiItems[droppedItemID];
        hotbarUiItems[draggedItemID].SwapWithData(droppedItemData.itemName, droppedItemData.itemCount, droppedItemData.itemImage.sprite, droppedItemData.isEmpty);

        inventoryUiItems[droppedItemID].SwapWithData(tempName, tempCount, tempSprite, tempisEmpty);

        DestroyDraggedObject();
    }

    internal void SwapUiItemInventoryToHotbar(int droppedItemID, int draggedItemID)
    {
        var tempName = inventoryUiItems[draggedItemID].itemName;
        var tempCount = inventoryUiItems[draggedItemID].itemCount;
        var tempSprite = inventoryUiItems[draggedItemID].itemImage.sprite;
        var tempisEmpty = inventoryUiItems[draggedItemID].isEmpty;

        var droppedItemData = hotbarUiItems[droppedItemID];
        inventoryUiItems[draggedItemID].SwapWithData(droppedItemData.itemName, droppedItemData.itemCount, droppedItemData.itemImage.sprite, droppedItemData.isEmpty);

        hotbarUiItems[droppedItemID].SwapWithData(tempName, tempCount, tempSprite, tempisEmpty);

        DestroyDraggedObject();
    }

    public void HighlightSelectedItem(int ui_id)
    {
        if (hotbarUiItems.ContainsKey(ui_id))
        {
            return;
        }
        inventoryUiItems[ui_id].ToggleHoghlight(true);
    }

    public void DisableHighlightForSelectedItem(int ui_id)
    {
        if (hotbarUiItems.ContainsKey(ui_id))
        {
            return;
        }
        inventoryUiItems[ui_id].ToggleHoghlight(false);
    }
}
