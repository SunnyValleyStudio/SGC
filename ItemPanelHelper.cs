using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPanelHelper : MonoBehaviour
{

    public Image itemImage;
    [SerializeField]
    protected Text nameText = null;
    public string itemName;
    public Outline outline;

    public Sprite backgroundSprite;

    protected virtual void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        if (itemImage.sprite == backgroundSprite)
        {
            ClearItem();
        }
    }

    public virtual void SetItemUI(string name, Sprite image)
    {
        itemName = name;
        nameText.text = itemName;
        SetImageSprite(image);
    }

    protected virtual void SetImageSprite(Sprite image)
    {
        itemImage.sprite = image;
    }

    public virtual void ClearItem()
    {
        itemName = "";
        nameText.text = itemName;
        ResetImage();
        ToggleHighlight(false);
    }

    protected virtual void ToggleHighlight(bool val)
    {
        outline.enabled = val;
    }

    protected virtual void ResetImage()
    {
        itemImage.sprite = backgroundSprite;
    }
    public virtual void ToggleHoghlight(bool val)
    {
        outline.enabled = val;
    }
}
