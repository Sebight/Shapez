using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Normal,
    Boss
}

public class Enemy
{
    public int health;
    public int damage;
    public int speed;
    public EnemyType type;
    public bool isStealth;
    
    public bool revealed;
    
    public virtual void Die()
    {
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
}
