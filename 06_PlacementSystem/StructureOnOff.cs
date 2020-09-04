using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureOnOff : Structure, IUsable
{
    [SerializeField]
    bool isUsable = true;
    public bool IsUsable => isUsable;

    public GameObject[] objectsToToggle;

    public void Use()
    {
        foreach (var objectToToggle in objectsToToggle)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }
    }
}
