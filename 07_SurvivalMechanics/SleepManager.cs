using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepManager : MonoBehaviour
{
    [Range(0.1f,1)]
    public float timeModifier = 1;
    public UiSleep uiSleep;
    private void Start()
    {
        uiSleep = GetComponent<UiSleep>();
    }
    internal void ShowUI()
    {
        Debug.Log("Freezing time");
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        uiSleep.Show();
    }

    public void SaveBed()
    {
        FindObjectOfType<AgentController>().SaveSpawnPoint();
    }

    public void Exit()
    {
        uiSleep.Hide();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Sleep()
    {
        uiSleep.ToggleAllButtons();
        StartCoroutine(SleepCoroutine(4));
    }

    IEnumerator SleepCoroutine(int seconds)
    {
        for (int i = 1; i <= seconds; i++)
        {
            yield return new WaitForSecondsRealtime(timeModifier);
            Debug.Log("Slept " + i+"/" + seconds + " hours");
        }
        ItemSpawnManager.instance.RespawnItems();
        Debug.Log("Restor player stats");
        uiSleep.ToggleAllButtons();
    }

}
