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
    public int lifesToRemove;
    private bool isSlowed = false;

    [Header("Spawn stuff")]
    public GameObject spawnEffect;

    [Header("Death stuff")]
    //public GameObject deathEffect;
    public bool IsDead;

    [Header("Health stuff")]
    public Slider healthSlider;
    public Image healthImage;
    public Color FullHealthColor = Color.green;
    public Color MidHealthColor = Color.yellow;
    public Color ZeroHealthColor = Color.red;

    [Header("Attack variables")]
    public float damage;
    public float rangeDetect = 2f;
    public float rangeAttack = 1f;

    [Header("Sounds")]
    public AudioClip deathSound;
    public AudioClip attackSound;


    //public float attackRate = 1f;
    //private float attackCountdown = 0f;
    [Header("Target Aquired")]
    public Transform target;
    private float turnSpeed = 10f;

    private GameManager gameManager;
    private ManagerUI managerUI;
    private EnemyMovement enemyMovement;
    private EnemyAnimation enemyAnimation;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        health = startHealth;
        gameManager = GameManager.instance;
        managerUI = gameManager.GetComponent<ManagerUI>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        enemyMovement = GetComponent<EnemyMovement>();
        audioSource = GetComponent<AudioSource>();
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

    public IEnumerator SetSlowForSeconds(int seconds, float movementSpeed)
    {
        isSlowed = true;        
        enemyMovement.SetSpeed(-movementSpeed);
        yield return new WaitForSeconds(seconds);
        enemyMovement.SetSpeed(movementSpeed);
        isSlowed = false;
    }

    public void TakeSlow(int seconds, float movementSpeed)
    {
        if (isSlowed) return;

        StartCoroutine(SetSlowForSeconds(seconds, movementSpeed));
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

        if (managerUI.effectFlag)
        {
            audioSource.clip = deathSound;
            audioSource.Play();
        }

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
        gameManager.KillMonster(1);

        //if (deathEffect != null)
        //{
        //    GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        //    Destroy(effect, 5f);
        //}

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
        if (target == null) return;
        enemyAnimation.PlayAttack();

        //if (attackCountdown <= 0f)
        //{
        //    if (target == null) return;
        //    enemyAnimation.PlayAttack();
        //    attackCountdown = 1f / attackRate;
        //}

        //attackCountdown -= Time.deltaTime;
    }

    private void Hit()
    {
        if (target == null) return;

        if (managerUI.effectFlag)
        {
            audioSource.clip = attackSound;
            audioSource.Play();
        }

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
