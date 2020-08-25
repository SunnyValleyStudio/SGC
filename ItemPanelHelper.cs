using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanelHelper : MonoBehaviour
{
    public Action<int, bool> OnClickEvent;

    public Image itemImage;
    [SerializeField]
    private Text nameText, countText;
    public string itemName, itemCount;
    public bool isEmpty = true;
    public Outline outline;
    public bool isHotbarItem = false;

    public Sprite backgroundSprite;

    private void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        if (itemImage.sprite == backgroundSprite)
        {
            ClearItem();
        }
    }

    private void ClearItem()
    {
        itemName = "";
        itemCount = "";
        countText.text = itemCount;
        if (!isHotbarItem)
            nameText.text = itemName;
        ResetImage();
        isEmpty = true;
        ToggleHighlight(false);
    }

    private void ToggleHighlight(bool val)
    {
        outline.enabled = val;
    }

    private void ResetImage()
    {
        itemImage.sprite = backgroundSprite;
    }
}
