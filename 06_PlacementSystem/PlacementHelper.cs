using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlacementHelper : MonoBehaviour
{
    Transform playerTransform;
    BoxCollider boxCollider;
    Rigidbody rb;
    public List<Collider> collisions = new List<Collider>();

    private float raycastMaxDistance = 5f;
    private float maxheightDifference = .3f;

    LayerMask layerMask;

    private void Start()
    {
        layerMask.value = 1 << LayerMask.NameToLayer("Ground");
    }

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

                RaycastHit hit1, hit2, hit3, hit4;
                bool result1 = Physics.Raycast(transform.TransformPoint(topLeftCorner) + Vector3.up, Vector3.down, out hit1, raycastMaxDistance, layerMask);
                bool result2 = Physics.Raycast(transform.TransformPoint(topRightCorner) + Vector3.up, Vector3.down, out hit2, raycastMaxDistance, layerMask);
                bool result3 = Physics.Raycast(transform.TransformPoint(bottomLeftCorner) + Vector3.up, Vector3.down, out hit3, raycastMaxDistance, layerMask);
                bool result4 = Physics.Raycast(transform.TransformPoint(bottomRightCorner) + Vector3.up, Vector3.down, out hit4, raycastMaxDistance, layerMask);

                if(result1 && result2 && result3 && result4)
                {
                    float[] heightValuesList = { hit1.point.y, hit2.point.y , hit3.point.y , hit4.point.y };
                    var min = heightValuesList.Min();
                    var max = heightValuesList.Max();
                    if(max-min > maxheightDifference)
                    {
                        Debug.Log("Too bigh height difference");
                        ChangeMaterialColor(Color.red);
                    }
                    else
                    {
                        Debug.Log("Placement position correct");
                        ChangeMaterialColor(Color.green);
                    }
                }
                //check the height difference

                //Change material color - user feedback
            }
        }
    }

    private void ChangeMaterialColor(Color color)
    {

    }

    internal void DestroyStructure()
    {
        Destroy(gameObject);
    }
}
