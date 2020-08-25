using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeItemHelper : ItemPanelHelper, IPointerClickHandler
{
    public Action<int> OnClickEvent;
    public void OnPointerClick(PointerEventData eventData)
    {
        int id = gameObject.GetInstanceID();
        OnClickEvent?.Invoke(id);
    }
}
