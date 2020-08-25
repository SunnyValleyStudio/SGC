using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public string ID;
    public string itemName;
    public Sprite imageSprite;
    public GameObject model;
    public bool isStackable;
    [Range(1,100)]
    public int stackLimit = 100;
    public ItemType itemType;
}

public enum ItemType
{
    None,
    Food,
    Weapon
}
