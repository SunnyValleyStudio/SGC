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

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("LoadSavedData") == 1)
        {
            StartCoroutine(saveSystem.LoadSavedDataCoroutine(DoneLoading));
            PlayerPrefs.SetInt("LoadSavedData", 0);
        }
    }

    private void DoneLoading()
    {
        Debug.Log("Data loaded");
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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
