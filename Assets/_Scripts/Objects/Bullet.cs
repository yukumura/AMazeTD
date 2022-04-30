using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public int damage = 50;
    public float speed = 70f;
    public float explosionRadius = 0f;
    public GameObject startEffect;
    public GameObject impactEffect;
    public AudioClip impactAudio;
    ManagerUI managerUI;

    /// <summary>
    /// Seek target
    /// </summary>
    /// <param name="_target"></param>
    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Use this for initialization
    void Start()
    {
        managerUI = GameManager.instance.GetComponent<ManagerUI>();

        if(startEffect != null)
        {
            Instantiate(startEffect, transform.position, transform.rotation);
        }
    }

    /// <summary>
    /// Reach object on time
    /// </summary>
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    /// <summary>
    /// When projectile hit the target
    /// </summary>
    private void HitTarget()
    {
        if(impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effect, 2f);
        }

        if(impactAudio != null && managerUI.effectFlag)
        {
            AudioSource.PlayClipAtPoint(impactAudio, transform.position);
        }

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// When the bullet make radius damage
    /// </summary>
    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var coll in colliders)
        {
            if(coll.tag == "Enemy")
            {
                Damage(coll.transform);
            }
        }
    }

    /// <summary>
    /// Call Enemy TakeDamage method
    /// </summary>
    /// <param name="enemyGO">Enemy Game Object</param>
    void Damage(Transform enemyGO)
    {
        Enemy enemy = enemyGO.GetComponent<Enemy>();

        if(enemy != null && !enemy.IsDead)
            enemy.TakeDamage(damage);
    }

    /// <summary>
    /// UI Radius
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
