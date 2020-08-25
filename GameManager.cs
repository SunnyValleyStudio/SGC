using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public SaveSystem saveSystem;
    public string mainMenuSceneName;
    public UiInGameMenu gameMenu;
    private bool timeAlreadyStopped = false;
    private bool pointerConfined = false;
    public AudioSource ambientSounds;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("LoadSavedData") == 1)
        {
            Time.timeScale = 0;
            gameMenu.ToggleLoadingPanel();
            StartCoroutine(saveSystem.LoadSavedDataCoroutine(DoneLoading));
            PlayerPrefs.SetInt("LoadSavedData", 0);
        }
    }

    public void ToggleGameMenu()
    {
        if (gameMenu.MenuVisible == false)
        {
            ambientSounds.Pause();
            if (Cursor.lockState == CursorLockMode.Confined)
            {
                pointerConfined = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            

            if (Time.timeScale == 0)
            {
                timeAlreadyStopped = true;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
        else
        {
            ambientSounds.UnPause();
            if (timeAlreadyStopped)
            {
                timeAlreadyStopped = false;
            }
            else
            {
                Time.timeScale = 1;
            }
            if (pointerConfined)
            {
                pointerConfined = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            

        }
        gameMenu.ToggleMenu();
    }

    internal void SaveGame()
    {
        saveSystem.SaveObjects();
    }

    private void DoneLoading()
    {
        gameMenu.ToggleLoadingPanel();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    internal void StartNextScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    internal void LoadSavedGame()
    {
        PlayerPrefs.SetInt("LoadSavedData", 1);
        StartNextScene();
    }

    internal bool CheckSavedGameExists()
    {
        return saveSystem.CheckSavedDataExists();
    }
}
