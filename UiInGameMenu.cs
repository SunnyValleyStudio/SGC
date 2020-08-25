using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInGameMenu : MonoBehaviour
{
    public Button saveBtn, exitBtn;
    public GameObject gameMenuPanel, loadingPanel;

    public bool MenuVisible { get => gameMenuPanel.activeSelf; }

    private void Start()
    {
        GameManager manager = FindObjectOfType<GameManager>();
        saveBtn.onClick.AddListener(manager.SaveGame);
        exitBtn.onClick.AddListener(manager.ExitToMainMenu);
        gameMenuPanel.SetActive(false);
    }

    public void ToggleMenu()
    {
        gameMenuPanel.SetActive(!gameMenuPanel.activeSelf);
    }

    public void ToggleLoadingPanel()
    {
        loadingPanel.SetActive(!loadingPanel.activeSelf);
    }
}
