using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemPanelHelper : ItemPanelHelper, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Action<int, bool> OnClickEvent;

    public Action<PointerEventData> DragStopCallback, DragContinueCallback;
    public Action<PointerEventData, int> DragStartCallback, DropCalback;

    [SerializeField]
    private Text countText = null;
    public int itemCount;
    public bool isEmpty = true;
    public bool isHotbarItem = false;

    public Image equippedIndicator;
    private bool equipped = false;


    public void SetItemUI(string name, int count, Sprite image)
    {
        itemName = name;
        itemCount = count;
        if (!isHotbarItem)
            nameText.text = itemName;
        if (count < 0)
        {
            countText.text = "";
        }
        else
        {
            countText.text = itemCount + "";
        }
        isEmpty = false;
        SetImageSprite(image);

        if (equipped)
        {
            ModifyEquippedIndicatorAlpha(1);
        }
        else
        {
            ModifyEquippedIndicatorAlpha(0);
        }
    }

    public override void SetItemUI(string name, Sprite image)
    {
        itemName = name;
        itemCount = 0;
        if (!isHotbarItem)
            nameText.text = itemName;
        countText.text = "";
        isEmpty = false;
        SetImageSprite(image);

        if (equipped)
        {
            ModifyEquippedIndicatorAlpha(1);
        }
        else
        {
            ModifyEquippedIndicatorAlpha(0);
        }
    }

    public void SwapWithData(string name, int count, Sprite image, bool isEmpty)
    {
        SetItemUI(name, count, image);
        this.isEmpty = isEmpty;
    }

    public override void ClearItem()
    {
        itemName = "";
        itemCount = -1;
        countText.text = "";
        if (!isHotbarItem)
            nameText.text = itemName;
        ResetImage();
        isEmpty = true;
        ToggleHighlight(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickEvent.Invoke(GetInstanceID(), isEmpty);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEmpty)
            return;
        DragStartCallback.Invoke(eventData, GetInstanceID());
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isEmpty)
            return;
        DragContinueCallback.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isEmpty)
            return;
        DragStopCallback.Invoke(eventData);
    }

    internal void UpdateCount(int count)
    {
        itemCount = count;
        countText.text = itemCount + "";
    }

    public void OnDrop(PointerEventData eventData)
    {
        DropCalback.Invoke(eventData, GetInstanceID());
    }

    public void ToggleEquippedIndicator()
    {
        if(equipped == false)
        {
            ModifyEquippedIndicatorAlpha(1);
            equipped = true;
        }
        else
        {
            ModifyEquippedIndicatorAlpha(0);
            equipped = false;
        }
    }

    private void ModifyEquippedIndicatorAlpha(int alpha)
    {
        Color c = equippedIndicator.color;
        c.a = Mathf.Clamp01(alpha);
        equippedIndicator.color = c;
    }
}
