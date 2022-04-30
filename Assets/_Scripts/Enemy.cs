using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Current Status")]
    public float startHealth = 100f;
    private float health;
    public int value = 50;

    [Header("Death stuff")]
    public GameObject deathEffect;
    public bool IsDead;

    [Header("Health stuff")]
    public Slider healthSlider;
    public Image healthImage;
    public Color FullHealthColor = Color.green;
    public Color MidHealthColor = Color.yellow;
    public Color ZeroHealthColor = Color.red;

    [Header("Attack variables")]
    public Transform target;
    public float rangeDetect = 2f;
    public float rangeAttack = 1f;
    public float attackRate = 1f;
    public float attackCountdown = 0f;
    public float turnSpeed = 10f;
    public bool isAttacking = false;
    public bool targetReached = false;
    public float damage;

    private GameManager gameManager;
    private EnemyMovement enemyMovement;
    private EnemyAnimation enemyAnimation;

    // Use this for initialization
    void Start()
    {
        health = startHealth;
        gameManager = GameManager.instance;

        enemyAnimation = GetComponent<EnemyAnimation>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {        
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        SetHealthUI();

        if (health <= 0)
        {
            Die();
        }
    }

    private void SetHealthUI()
    {
        healthSlider.value = health / startHealth;
        healthImage.color = healthSlider.value > .8f ? FullHealthColor : healthSlider.value < .4 ? ZeroHealthColor : MidHealthColor;
    }

    private void Die()
    {
        healthSlider.transform.parent.gameObject.SetActive(false);
        enemyAnimation.PlayDead();

        if (!enemyMovement.isFlyingCreature)
        {
            enemyMovement.StopMoving();
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }

        
        IsDead = true;
        gameManager.AddGold(value);
        gameManager.AddMonster(-1);

        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
        }


        if(target != null)
        {
            Wall wall = target.GetComponent<Wall>();
            if (wall != null)
            {
                wall.IsAttacked = false;
            }

            Turret turret = target.GetComponent<Turret>();
            if (turret != null)
            {
                turret.IsAttacked = false;
            }
        }

        Destroy(gameObject, 5f);
    }

    public void FindNearEnemy()
    {
        //trovare metodo per bindare un target e fare in modo che gli altri nemici puntino altri punti
        //if(target == null)
            FindTarget();

        if (target == null)
        {
            enemyMovement.ReachGoalDestination();
        }
        else
        {
            if (enemyMovement.InRange(target.position))
            {                
                enemyMovement.StopMoving();
                AimTarget();
                Attack();
            }
            else
            {
                enemyMovement.SetDestination(target.position);
            }
        }                   
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
            turret.TakeDamage(damage);
        }
    }

    void FindTarget()
    {        
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Wall").Union(GameObject.FindGameObjectsWithTag("Turret")).ToArray();

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

        if (nearestObject != null && shortestDistance <= rangeDetect)
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
        Gizmos.DrawWireSphere(transform.position, rangeDetect);
    }
}
