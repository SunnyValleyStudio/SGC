using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementHelper : MonoBehaviour
{
    Transform playerTransform;
    BoxCollider boxCollider;
    Rigidbody rb;
    public List<Collider> collisions = new List<Collider>();

    public void Initialize(Transform transform)
    {
        playerTransform = transform;
    }

    public void PrepareForMovement()
    {
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(playerTransform != null)
        {
            rb.position = playerTransform.position + playerTransform.forward;
            rb.MoveRotation(Quaternion.LookRotation(playerTransform.forward));
        }
    }
}
