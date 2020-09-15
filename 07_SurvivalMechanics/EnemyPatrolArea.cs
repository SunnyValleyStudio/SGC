using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyPatrolArea : MonoBehaviour
{
    public Action OnPlayerInRange, OnPlayerExit;
    public float patrolRadius = 5;
    public SphereCollider sphereCollider;
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = patrolRadius;
        sphereCollider.isTrigger = true;
    }

    private void OnDrawGizmos()
    {
        if (patrolRadius > 0)
        {
            Gizmos.DrawWireSphere(transform.position, patrolRadius);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in range");
            OnPlayerInRange?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left area");
            OnPlayerExit?.Invoke();
        }
    }


}
