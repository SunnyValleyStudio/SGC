using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructuresInteractionManager : MonoBehaviour
{
    public SleepManager sleepManager;

    public void ShowSLeepUI()
    {
        sleepManager.ShowUI();
    }
}
