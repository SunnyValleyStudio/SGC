using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientItemElement : ItemPanelHelper
{
    public Image panelImage;
    public Text countNeedeTxt;

    private void ModifyPanelAlpha(int value)
    {
        var panelColor = panelImage.color;
        panelColor.a = Mathf.Clamp01(value);
        panelImage.color = panelColor;
    }

    public void SetUnavailable()
    {
        ModifyPanelAlpha(1);
    }

    public void ResetAvailability()
    {
        ModifyPanelAlpha(0);
    }

    public override void SetItemUI(string name, Sprite image)
    {
        base.SetItemUI(name, image);
        countNeedeTxt.text = "x 0";
    }

    public void SetItemUI(string name, Sprite image, int count, bool enoughItems)
    {
        base.SetItemUI(name, image);
        countNeedeTxt.text = "x "+count;
        if (enoughItems)
        {
            ResetAvailability();
        }
        else
        {
            SetUnavailable();
        }
    }
}
