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

    List<Material[]> objectMaterials = new List<Material[]>();

    private float raycastMaxDistance = 5f;
    private float maxheightDifference = .3f;

    LayerMask layerMask;

    Material m_material;

    float lowestYHeight = 0;

    public bool CorrectLocation { get; private set; }
    private bool stopMovement = false;

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
        MaterialHelper.SwapToSelectionMaterial(gameObject, objectMaterials, ItemSpawnManager.instance.transparentMaterial);
        m_material = GetComponent<Renderer>().material;
    }

    public Structure PrepareForPlacement()
    {
        stopMovement = true;
        MaterialHelper.SwapToOriginalMaterial(gameObject, objectMaterials);
        Destroy(rb);
        boxCollider.isTrigger = false;
        var structureComponent = GetComponent<Structure>();
        if(structureComponent == null)
        {
            structureComponent = gameObject.AddComponent<Structure>();
        }
        return structureComponent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Pickable") && other.gameObject.layer != LayerMask.NameToLayer("Player") && other.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            if(collisions.Contains(other)!= true)
            {
                collisions.Add(other);
                ChangeMaterialColor(Color.red);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collisions.Remove(other);
        if(collisions.Count == 0)
        {
            ChangeMaterialColor(Color.green);
        }
    }

    private void FixedUpdate()
    {
        if(stopMovement == false && playerTransform != null)
        {
            var positionToMove = playerTransform.position + playerTransform.forward;
            rb.position = positionToMove;
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
                    if(min < lowestYHeight)
                    {
                        ChangeMaterialColor(Color.red);
                        Debug.Log("Cant place that low");
                        CorrectLocation = false;
                    }
                    else if(max-min > maxheightDifference)
                    {
                        Debug.Log("Too bigh height difference");
                        ChangeMaterialColor(Color.red);
                        CorrectLocation = false;
                    }
                    else
                    {
                        Debug.Log("Placement position correct");
                        ChangeMaterialColor(Color.green);
                        rb.position = new Vector3(positionToMove.x, (max + min) / 2f, positionToMove.z);
                        CorrectLocation = true;
                    }

                }
                //check the height difference

                //Change material color - user feedback
            }
        }
    }

    private void ChangeMaterialColor(Color color)
    {
        m_material.SetColor("Color_Shader", color);

    }

    internal void DestroyStructure()
    {
        Destroy(gameObject);
    }
}
