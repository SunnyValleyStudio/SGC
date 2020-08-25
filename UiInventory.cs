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

    public bool IsInventoryVisible { get => inventoryGeneralPanel.activeSelf; }
    public int HotbarElementsCount { get =>hotbarUiItems.Count;}
    public RectTransform Draggableitem { get => draggableitem; }
    public ItemPanelHelper DraggableItemPanel { get => draggableItemPanel; }

    public Dictionary<int, ItemPanelHelper> inventoryUiItems = new Dictionary<int, ItemPanelHelper>();
    public Dictionary<int, ItemPanelHelper> hotbarUiItems = new Dictionary<int, ItemPanelHelper>();

    private List<int> listOfHotbarElementsID = new List<int>();

    public List<ItemPanelHelper> GetUiElementsForHotbar()
    {
        return hotbarUiItems.Values.ToList();
    }

    public GameObject hotbarPanel, storagePanel;

    public GameObject storagePrefab;

    private RectTransform draggableitem;
    private ItemPanelHelper draggableItemPanel;

    public Canvas canvas;

    private void Awake()
    {
        inventoryGeneralPanel.SetActive(false);
        foreach (Transform child in hotbarPanel.transform)
        {
            ItemPanelHelper helper = child.GetComponent<ItemPanelHelper>();
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
        }
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
            var itemHelper = element.GetComponent<ItemPanelHelper>();
            inventoryUiItems.Add(itemHelper.GetInstanceID(), itemHelper);
        }
    }

    public List<ItemPanelHelper> GetUiElementsForInventory()
    {
        return inventoryUiItems.Values.ToList();
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
}
