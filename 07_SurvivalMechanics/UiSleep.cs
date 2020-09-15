using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSleep : MonoBehaviour
{
    public GameObject sleepPanel;
    public Button[] buttons;

    private void Start()
    {
        Hide();
    }
    internal void Show()
    {
        sleepPanel.SetActive(true);
    }

    internal void Hide()
    {
        sleepPanel.SetActive(false);
    }

    internal void ToggleAllButtons()
    {
        foreach (var button in buttons)
        {
            button.interactable = !button.interactable;
        }
    }
}
