using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyPatrolArea enemyPatrolArea;
    private void Start()
    {
        enemyPatrolArea.OnPlayerInRange += ChasePlayer;
        enemyPatrolArea.OnPlayerExit += StopChasing;

    }

    private void StopChasing()
    {

    }

    private void ChasePlayer()
    {

    }

    public void Attack()
    {

    }

    public void Die()
    {

    }

}
