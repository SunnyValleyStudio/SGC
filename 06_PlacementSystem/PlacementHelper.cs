using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementHelper : MonoBehaviour
{
    Transform playerTransform;
    BoxCollider boxCollider;
    Rigidbody rb;
    public List<Collider> collisions = new List<Collider>();

    private float raycastMaxDistance = 5f;

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
            if(collisions.Count == 0)
            {
                //find corners of box collider
                Vector3 bottomCenter = new Vector3(boxCollider.center.x, boxCollider.center.y - boxCollider.size.y / 2f,boxCollider.center.z);

                Vector3 topLeftCorner = bottomCenter + new Vector3(-boxCollider.size.x / 2f, 0, boxCollider.size.z / 2f);
                Vector3 topRightCorner = bottomCenter + new Vector3(+boxCollider.size.x / 2f, 0, boxCollider.size.z / 2f);
                Vector3 bottomLeftCorner = bottomCenter + new Vector3(-boxCollider.size.x / 2f, 0, -boxCollider.size.z / 2f);
                Vector3 bottomRightCorner = bottomCenter + new Vector3(boxCollider.size.x / 2f, 0, -boxCollider.size.z / 2f);

                //shoot rays from thos points

                Debug.DrawRay(transform.TransformPoint(topLeftCorner) + Vector3.up, Vector3.down * raycastMaxDistance, Color.magenta);
                Debug.DrawRay(transform.TransformPoint(topRightCorner) + Vector3.up, Vector3.down * raycastMaxDistance, Color.magenta);
                Debug.DrawRay(transform.TransformPoint(bottomLeftCorner) + Vector3.up, Vector3.down * raycastMaxDistance, Color.magenta);
                Debug.DrawRay(transform.TransformPoint(bottomRightCorner) + Vector3.up, Vector3.down * raycastMaxDistance, Color.magenta);

                //check the height difference

                //Change material color - user feedback
            }
        }
    }

    internal void DestroyStructure()
    {
        Destroy(gameObject);
    }
}
