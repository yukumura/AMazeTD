using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float health = 100f;
    public bool IsDead;
    public GameObject deathEffect;

    private Transform target;
    private Enemy targetEnemy;

    private Animator anim;

    [Header("General")]
    public float range = 15f;
    public bool IsAttacked = false;

    [Header("Audio")]
    public AudioClip attack;
    public AudioClip destroy;
    private AudioSource audiosource;

    [Header("Animation to Attack")]
    public bool animToAttack;
    public string animToAttackName;
    public string animToIdle;

    [Header("Bullets")]
    public float fireRate = 1f;
    public float fireCountdown = 0f;
    public GameObject bulltetPrefab;

    [Header("Laser")]
    public bool useLaser;
    public int damageOverTime = 30;
    public bool isLaserSlow = false;
    public float laserSlow = .5f;
    public int laserSlowSeconds = 2;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;

    [Header("Unity Setup Fields")]
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public string enemyTag;

    public Transform firePoint;
    GameManager gameManager;
    ManagerUI managerUI;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.4f);
        anim = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        gameManager = GameManager.instance;
        managerUI = gameManager.GetComponent<ManagerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.LoseFlag) return;

        if (target == null)
        {
            //Turn off laser
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    if (impactEffect != null)
                        impactEffect.Stop();

                    audiosource.volume = 0;
                }
            }
            return;
        }

        AimTarget();

        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (fireCountdown <= 0f)
            {
                if (animToAttack)
                {
                    anim.Play(animToAttackName);
                }
                else
                {
                    Shoot();
                }

                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }

    }

    //If the turret use laser, use laser effects + make damage over time
    private void Laser()
    {
        if (targetEnemy.IsDead) return;

            targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);

        if(managerUI.effectFlag && !managerUI.pauseFlag)
            audiosource.volume = 0.1f;

        if (isLaserSlow)
        {
            targetEnemy.TakeSlow(laserSlowSeconds, laserSlow);
        }

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            if (impactEffect != null)
                impactEffect.Play();
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        if (impactEffect != null)
        {
            impactEffect.transform.position = target.position + dir.normalized;
            impactEffect.transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    /// <summary>
    /// If the turret isn't a laser, shoot
    /// </summary>
    private void Shoot()
    {
        if (attack != null && managerUI.effectFlag)
        {
            audiosource.clip = attack;
            audiosource.Play();
        }

        GameObject bulletGO = Instantiate(bulltetPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
            bullet.Seek(target);
    }

    private void AimTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    /// <summary>
    /// Check nearby enemy to shot / laser
    /// </summary>
    void UpdateTarget()
    {
        if (target != null)
        {
            Enemy enemy = target.GetComponent<Enemy>();

            if(!enemy.IsDead && Vector3.Distance(transform.position, target.position) < range) return;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>().IsDead) continue;

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = target.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
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

        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
        }

        gameObject.GetComponentInParent<Tile>().DestroyObject();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
