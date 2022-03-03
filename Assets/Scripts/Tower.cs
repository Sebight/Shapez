using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameManager gameManager;

    public float range = 10f;
    public float fireRate = 1f;
    public float damage = 1f;
    public float cost = 10f;

    private float nextAttackTime;

    public virtual void OnPlaced()
    {
    }

    public virtual void OnRemoved()
    {
    }

    public virtual void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public Enemy GetNearestEnemy()
    {
        Enemy closestEnemy = null;
        float closestDistance = 200;
        for (int i = 0; i < gameManager.enemies.Count; i++)
        {
            Enemy currentEnemy = gameManager.enemies[i];
            if (Vector3.Distance(transform.position, currentEnemy.transform.position) < closestDistance)
            {
                closestEnemy = currentEnemy;
                closestDistance = Vector3.Distance(transform.position, currentEnemy.transform.position);
            }
        }

        return closestEnemy;
    }

    public virtual void Attack()
    {
        if (nextAttackTime <= Time.time)
        {
            nextAttackTime = Time.time + fireRate;
            Enemy nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null && Vector3.Distance(transform.position, nearestEnemy.transform.position) <= range)
            {
                nearestEnemy.TakeDamage(1);
            }
        }
    }
}