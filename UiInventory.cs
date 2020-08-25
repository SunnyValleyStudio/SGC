using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiInventory : MonoBehaviour
{
    public GameObject inventoryGeneralPanel;

    public bool IsInventoryVisible { get => inventoryGeneralPanel.activeSelf; }
    public int HotbarElementsCount { get =>hotbarUiItems.Count;}

    public Dictionary<int, ItemPanelHelper> inventoryUiItems = new Dictionary<int, ItemPanelHelper>();
    public Dictionary<int, ItemPanelHelper> hotbarUiItems = new Dictionary<int, ItemPanelHelper>();

    private List<int> listOfHotbarElementsID = new List<int>();

    public List<ItemPanelHelper> GetUiElementsForHotbar()
    {
        return hotbarUiItems.Values.ToList();
    }

    public GameObject hotbarPanel;

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
}
