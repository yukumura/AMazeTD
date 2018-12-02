using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public float health = 100f;
    public int value = 50;

    public GameObject deathEffect;
    public string deathAnimation;

    public bool IsDead;

    [Header("Attack variables")]
    public Transform target;
    public float range = 1f;
    public float attackRate = 1f;
    public float attackCountdown = 0f;
    public float turnSpeed = 10f;
    public string objectsTag;
    public bool isAttacking = false;
    public bool targetReached = false;
    public float damage;

    private GameManager gameManager;
    //private EnemyMovement enemyMovement;
    private EnemyAnimation enemyAnimation;

    [Header("Movement variables")]

    //try to move enemy movement here
    public GameObject GoalPosition;
    private NavMeshAgent myNavMeshAgent;
    private Enemy enemy;


    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.instance;

        enemyAnimation = GetComponent<EnemyAnimation>();
        //enemyMovement = GetComponent<EnemyMovement>();

        //try to move enemy movement here
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        SetGoalDestination();
    }

    // Update is called once per frame
    void Update()
    {
        //try to move enemy movement here
        Debug.Log("is stopped: " + myNavMeshAgent.isStopped);
        if (enemy.IsDead)
        {
            myNavMeshAgent.enabled = false;
        }

        NavMeshPath path = new NavMeshPath();
        myNavMeshAgent.CalculatePath(GoalPosition.transform.position, path);

        switch (path.status)
        {
            case NavMeshPathStatus.PathComplete:
                ReachGoalDestination();
                break;
            case NavMeshPathStatus.PathPartial:
                Debug.Log("Warning: path cannot reach destination");
                enemy.FindNearEnemy();
                break;
            case NavMeshPathStatus.PathInvalid:
                break;
        }
    }

    public void ReachDestination(Vector3 pos)
    {
        myNavMeshAgent.isStopped = false;
        enemyAnimation.PlayWalk();
        myNavMeshAgent.SetDestination(pos);
    }

    public void SetGoalDestination()
    {
        GoalPosition = GameObject.FindGameObjectWithTag("Goal");
        ReachGoalDestination();
    }

    public void ReachGoalDestination()
    {
        ReachDestination(GoalPosition.transform.position);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;

        gameManager.AddGold(value);
        gameManager.AddMonster(-1);

        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
        }

        if (!string.IsNullOrEmpty(deathAnimation))
        {
            enemyAnimation.PlayDead();
        }

        Destroy(gameObject, 5f);
    }

    public void FindNearEnemy()
    {
        FindTarget();

        if (target == null)
        {
            enemyMovement.ReachGoalDestination();
        }
        else
        {
            //enemyMovement.SetTarget(target.position);

            enemyMovement.StopMoving();
            AimTarget();
            Attack();

            //if (enemyMovement.IsReachDestination())
            //{
                
            //}
            //else
            //{
            //    enemyMovement.ReachDestination(target.position);
            //}



            //if (!enemyMovement.IsMoving() && !enemyMovement.IsReachDestination())
            //    enemyMovement.ReachDestination(target.position);

            //if (enemyMovement.IsReachDestination())                           
        }
        //if (target == null)
        //{
        //    Debug.Log("target is null");
        //    isAttacking = false;
        //    targetReached = false;
        //    enemyMovement.ReachGoalDestination();
        //    return;
        //}

        //if (enemyMovement.IsReachDestination() && !targetReached)
        //{
        //    targetReached = true;
        //    enemyMovement.SetDestination(transform.position);
        //    Debug.Log("enemy reached");
        //    AimTarget();
        //    Attack();
        //}
        //else
        //{
        //    Debug.Log("target is far away. try to reach it");
        //    enemyMovement.SetDestination(target.position);
        //}                
    }

    private void Attack()
    {
        if (attackCountdown <= 0f)
        {
            if (target == null) return;
            enemyAnimation.PlayAttack();
            attackCountdown = 1f / attackRate;
        }

        attackCountdown -= Time.deltaTime;
    }

    private void Hit()
    {
        if (target == null) return;
        Wall wall = target.GetComponent<Wall>();

        if (wall != null)
        {
            wall.TakeDamage(damage);
        }

        Turret turret = target.GetComponent<Turret>();
        if (turret != null)
        {

        }
    }

    void FindTarget()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(objectsTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestObject = null;

        foreach (GameObject obj in objects)
        {
            float distanceToObject = Vector3.Distance(transform.position, obj.transform.position);
            if (distanceToObject < shortestDistance)
            {
                shortestDistance = distanceToObject;
                nearestObject = obj;
            }
        }

        if (nearestObject != null && shortestDistance <= range)
        {
            target = nearestObject.transform;
        }
        else
        {
            target = null;
        }
    }


    private void AimTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }


}
