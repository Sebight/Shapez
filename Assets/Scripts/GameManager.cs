using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gamemode
{
    Normal,
    Infinite
}

public class GameManager : MonoBehaviour
{
    public List<Tower> towersPlaced = new List<Tower>();
    public List<GameObject> enemiesPrefabs = new List<GameObject>();
    public List<GameObject> towersPrefabs;

    public List<Enemy> enemies = new List<Enemy>();

    [Header("Starting money")] public int startingMoney;
    
    public int money;
    
    //Health left for the player
    public int health = 100;

    public Gamemode gamemode;

    public Transform pathWaypoints;

    private bool waveRunning = false;
    private bool gameStarted = false;

    //Managers
    public UIManager uiManager;
    public WaveManager waveManager;
    public Leaderboard leaderboard;
    public InteractionManager interactionManager;
    public Settings settings;
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
        //! uncomment
        // yield return new WaitForSeconds(4);
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

    public IEnumerator SpawnWave(List<Enemy> enemy)
    {
        yield return new WaitForSeconds(4);
        for (int i = 0; i < enemy.Count; i++)
        {
            yield return new WaitForSeconds(0.3f);
            SpawnEnemy(enemy[i]);
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

        if (!waveRunning && gameStarted)
        {
            if (gamemode == Gamemode.Normal)
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
            else if (gamemode == Gamemode.Infinite)
            {
                if (enemies.Count == 0)
                {
                    waveRunning = true;
                    waveManager.IncrementWave();
                    uiManager.UpdateWaveText();
                    waveManager.InfiniteWave();
                }
            }
        }
    }

    public void SetTimeScale(int n) => Time.timeScale = n;
    
    public void DamagePlayer(int damage)
    {
        health -= damage;
        uiManager.UpdateHealthText(health);
        if (health <= 0)
        {
            health = 0;
            GameOver();
        }
    }

    public void GameOver()
    {
        StartCoroutine(leaderboard.AddEntry(new LeaderboardEntry(PlayerPrefs.GetString("username"), waveManager.GetCurrentWave())));
        waveManager.Reset();
        uiManager.DisplayYouLost();
        interactionManager.ResetSelect();
        interactionManager.ResetPlacing();

        gameStarted = false;
        waveRunning = false;

        //Destroy all towers and all enemies
        for (int i = 0; i < towersPlaced.Count; i++)
        {
            Destroy(towersPlaced[i].gameObject);
        }

        towersPlaced.Clear();
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i].gameObject);
        }

        enemies.Clear();

        foreach (var prefab in towersPrefabs)
        {
            prefab.SetActive(false);
        }

        health = 100;
    }

    public void RemoveTower(Tower t)
    {
        towersPlaced.Remove(t);
        Destroy(t.gameObject);
    }

    public void StartGame()
    {

        foreach (var enemy in enemiesPrefabs)
        {
            enemy.GetComponent<Enemy>().ResetUpgradedStats();
        }

        health = 100;
        uiManager.UpdateHealthText(health);
        money = startingMoney;
        uiManager.UpdateMoneyText(money);
        gameStarted = true;
        Time.timeScale = 1;
        uiManager.ResetSpeedButtonState();
    }

    public void RegisterTower(Tower tower)
    {
        UpdateMoney(-tower.cost);
        uiManager.UpdateMoneyText(money);
        towersPlaced.Add(tower);
        tower.enabled = true;
        tower.Initialize(this);
    }

    [ContextMenu("Clear PlayerPrefs")]
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}