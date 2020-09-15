using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyPatrolArea enemyPatrolArea;
    public NavMeshAgent navMehsAgent;
    public GameObject target;
    public float stoppinfgDistance = 2;
    private bool playerInRange = false;

    Animator animator;

    private void Start()
    {
        enemyPatrolArea.OnPlayerInRange += ChasePlayer;
        enemyPatrolArea.OnPlayerExit += StopChasing;
        animator = GetComponent<Animator>();
        navMehsAgent = GetComponent<NavMeshAgent>();
        navMehsAgent.isStopped = true;
        navMehsAgent.stoppingDistance = stoppinfgDistance;
    }

    private void StopChasing()
    {
        animator.SetBool("Walk", false);
        navMehsAgent.isStopped = true;
        playerInRange = false;
    }

    private void ChasePlayer()
    {
        animator.SetBool("Walk", true);
        navMehsAgent.isStopped = false;
        playerInRange = true;
    }

    public void Attack()
    {
        target.GetComponent<AgentController>().playerStatsManager.Health -= 10;
    }

    public void Die()
    {

    }

    private void Update()
    {
        if(navMehsAgent.isStopped == false)
        {
            navMehsAgent.SetDestination(target.transform.position);
        }
        if(playerInRange && navMehsAgent.velocity.sqrMagnitude == 0)
        {
            transform.LookAt(target.transform);
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }
}
