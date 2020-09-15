using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSavePosition : Structure, IUsable
{
    public void Use()
    {
        FindObjectOfType<StructuresInteractionManager>().ShowSLeepUI();
    }
}
