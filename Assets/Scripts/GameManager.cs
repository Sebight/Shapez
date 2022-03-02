using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Tower> towersPlaced = new List<Tower>();
    public List<GameObject> enemiesPrefabs = new List<GameObject>();
    public List<Enemy> enemies = new List<Enemy>();

    public Transform pathWaypoints;
    
    //Health left for the player
    public int health = 100;

    public Transform GetPathWaypoints() => pathWaypoints;

    public void RemoveEnemy(Enemy e)
    {
        enemies.Remove(e);
        Destroy(e.gameObject);
    }

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemiesPrefabs[UnityEngine.Random.Range(0, enemiesPrefabs.Count)], pathWaypoints.GetChild(0).position, Quaternion.identity);
        Enemy e = enemy.GetComponent<Enemy>();
        e.Initialize(this);
        enemies.Add(e);
    }

    public void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0.1f, 0.5f);
    }
    
    public void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Navigate();
        }
    }
    
}
