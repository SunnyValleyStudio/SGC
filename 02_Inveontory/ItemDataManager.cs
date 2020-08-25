using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    Dictionary<string, ItemSO> itemsDictionary = new Dictionary<string, ItemSO>();
    public List<ItemSO> itemSoList = new List<ItemSO>();

    public static ItemDataManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        foreach (var item in itemSoList)
        {
            itemsDictionary.Add(item.ID, item);
        }
    }

    public string GetItemName(string id)
    {
        if (itemsDictionary.ContainsKey(id) == false)
        {
            throw new System.Exception("ItemDataManage doesn't have " + id);
        }
        return itemsDictionary[id].itemName;
    }

    public Sprite GetItemSprite(string id)
    {
        if (itemsDictionary.ContainsKey(id) == false)
        {
            throw new System.Exception("ItemDataManage doesn't have " + id);
        }
        return itemsDictionary[id].imageSprite;
    }

    public ItemSO GetItemData(string id)
    {
        if (itemsDictionary.ContainsKey(id) == false)
        {
            throw new System.Exception("ItemDataManage doesn't have " + id);
        }
        return itemsDictionary[id];
    }

    public GameObject GetItemPrefab(string id)
    {
        if (itemsDictionary.ContainsKey(id) == false)
        {
            throw new System.Exception("ItemDataManage doesn't have " + id);
        }
        return itemsDictionary[id].model;
    }

    public bool IsItemUsabel(string id)
    {
        if (itemsDictionary.ContainsKey(id) == false)
        {
            throw new System.Exception("ItemDataManage doesn't have " + id);
        }
        return itemsDictionary[id].IsUsable();
    }
}
