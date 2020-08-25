using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public ItemSO itemToSpawn;
    [Range(1,100)]
    public int count = 1;
    public bool singleObject = false;

    [Range(0.1f, 50f)]
    public float radius = 1;
    public bool showGizmo = true;
    public Color gizmoColor = Color.green;

    public void OnDrawGizmos()
    {
        if(showGizmo && radius > 0)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
