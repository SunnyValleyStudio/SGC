using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPanelHelper : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Action<int, bool> OnClickEvent;

    public Action<PointerEventData> DragStopCallback, DragContinueCallback;
    public Action<PointerEventData, int> DragStartCallback, DropCalback;

    public Image itemImage;
    [SerializeField]
    private Text nameText, countText;
    public string itemName;
    public int itemCount;
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

    public void SetInventoryUiElement(string name, int count, Sprite image)
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
    }

    public void SwapWithData(string name, int count, Sprite image, bool isEmpty)
    {
        SetInventoryUiElement(name, count, image);
        this.isEmpty = isEmpty;
    }

    private void SetImageSprite(Sprite image)
    {
        itemImage.sprite = image;
    }

    private void ClearItem()
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

    private void ToggleHighlight(bool val)
    {
        outline.enabled = val;
    }

    private void ResetImage()
    {
        itemImage.sprite = backgroundSprite;
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

    public void OnDrop(PointerEventData eventData)
    {
        DropCalback.Invoke(eventData, GetInstanceID());
    }
}
