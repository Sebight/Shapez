using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum EnemyType
{
    Normal,
    Stealth,
    Boss
}

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;

    public Transform pathWaypoints;

    public int health;
    public int damage;
    public int speed;
    public EnemyType type;

    private int currentWaypoint = 0;

    public virtual void Die()
    {
        gameManager.RemoveEnemy(this);
    }

    public virtual void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public virtual void Navigate()
    {
        if (pathWaypoints == null) pathWaypoints = gameManager.GetPathWaypoints();
        transform.position = Vector3.MoveTowards(transform.position, pathWaypoints.GetChild(currentWaypoint).position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, pathWaypoints.GetChild(currentWaypoint).position) < 0.1f)
        {
            currentWaypoint++;
            if (currentWaypoint >= pathWaypoints.childCount)
            {
                FinishPath();
            }
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void FinishPath()
    {
        gameManager.DamagePlayer(damage);
        gameManager.RemoveEnemy(this);
    }
}