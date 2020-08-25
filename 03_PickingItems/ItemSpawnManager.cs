using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{

    public static ItemSpawnManager instance;
    public Transform playerTransform;
    public string pickableLayerMask;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void CreateItemAtPlayersFeet(string itemID, int currentItemCount)
    {
        var itemPrefab = ItemDataManager.instance.GetItemPrefab(itemID);
        var itemGameObject = Instantiate(itemPrefab, playerTransform.position + Vector3.up, Quaternion.identity);
        PrepareItemGameObject(itemID, currentItemCount, itemGameObject);
    }

    private void PrepareItemGameObject(string itemID, int currentItemCount, GameObject itemGameObject)
    {
        itemGameObject.AddComponent<BoxCollider>();
        var rb = itemGameObject.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        var pickableItem = itemGameObject.AddComponent<PickableItem>();
        pickableItem.SetCount(currentItemCount);
        pickableItem.dataSource = ItemDataManager.instance.GetItemData(itemID);
        itemGameObject.layer = LayerMask.NameToLayer(pickableLayerMask);
    }
}
