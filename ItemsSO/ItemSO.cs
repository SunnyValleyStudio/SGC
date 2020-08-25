using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject, ISerializationCallbackReceiver
{
    public string ID;
    public string itemName;
    public Sprite imageSprite;
    public GameObject model;
    public bool isStackable;
    [Range(1,100)]
    public int stackLimit = 100;
    public ItemType itemType;

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        if (string.IsNullOrEmpty(this.ID))
        {
            this.ID = Guid.NewGuid().ToString("N");
        }
        if (string.IsNullOrEmpty(itemName) && model != null)
        {
            itemName = model.name;
        }
    }

    public Sprite GetImage()
    {
        return imageSprite;
    }

    public ItemType GetItemType()
    {
        return itemType;
    }

    public virtual bool IsUsable()
    {
        return false;
    }
}

public enum ItemType
{
    None,
    Food,
    Weapon,
    Material,
    Structure
}
