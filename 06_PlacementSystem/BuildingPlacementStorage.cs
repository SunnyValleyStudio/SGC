using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementStorage : MonoBehaviour
{
    List<Structure> playerStructures = new List<Structure>();

    public void SaveStructureReference(Structure structure)
    {
        playerStructures.Add(structure);
    }
}
