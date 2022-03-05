using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Tower> towersPlaced = new List<Tower>();
    public List<GameObject> enemiesPrefabs = new List<GameObject>();
    public List<Enemy> enemies = new List<Enemy>();
    public int money;
    //Health left for the player
    public int health = 100;
    public Transform pathWaypoints;

    private bool waveRunning = false;

    //Managers
    public UIManager uiManager;
    public WaveManager waveManager;

    public InteractionManager interactionManager;

    //Getters
    public Transform GetPathWaypoints() => pathWaypoints;

    public int GetMoney() => money;

    public int GetWave() => waveManager.GetCurrentWave();

    //

    public void RemoveEnemy(Enemy e)
    {
        enemies.Remove(e);
        Destroy(e.gameObject);
        if (enemies.Count == 0) waveRunning = false;
    }

    public void UpdateMoney(int amount)
    {
        money += amount;
        uiManager.UpdateMoneyText(money);
    }

    public void SpawnEnemy(Enemy enemyType)
    {
        GameObject enemy = Instantiate(enemyType.gameObject, pathWaypoints.GetChild(0).position, Quaternion.identity);
        // GameObject enemy = Instantiate(enemiesPrefabs[UnityEngine.Random.Range(0, enemiesPrefabs.Count)], pathWaypoints.GetChild(0).position, Quaternion.identity);
        Enemy e = enemy.GetComponent<Enemy>();
        e.Initialize(this);
        enemies.Add(e);
    }

    public IEnumerator SpawnWave(WaveDefinition wave)
    {
        yield return new WaitForSeconds(4);
        List<WaveDefinitionElement> waveDefinition = wave.definition;
        for (int i = 0; i < waveDefinition.Count; i++)
        {
            WaveDefinitionElement enemyTypeAndCount = waveDefinition[i];
            for (int j = 0; j < enemyTypeAndCount.count; j++)
            {
                yield return new WaitForSeconds(0.3f);
                SpawnEnemy(enemyTypeAndCount.enemyType);
            }
        }
    }

    public void Start()
    {
        uiManager.Initialize(this, money);
        foreach (var tower in towersPlaced)
        {
            tower.Initialize(this);
        }
    }

    public void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Navigate();
        }

        for (int i = 0; i < towersPlaced.Count; i++)
        {
            towersPlaced[i].Attack();
        }

        if (!waveRunning)
        {
            if (enemies.Count == 0)
            {
                waveRunning = true;
                waveManager.IncrementWave();
                uiManager.UpdateWaveText();
                WaveDefinition wave = waveManager.GetCurrentWaveDefinition();
                StartCoroutine(SpawnWave(wave));
            }
        }

    }

    public void DamagePlayer(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            GameOver();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }

    public void RegisterTower(Tower tower)
    {
        UpdateMoney(-tower.cost);
        uiManager.UpdateMoneyText(money);
        towersPlaced.Add(tower);
        tower.enabled = true;
        tower.Initialize(this);
    }
}