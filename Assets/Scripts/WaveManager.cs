using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveDefinitionElement
{
    [Header("Wave enemy")]
    public Enemy enemyType;
    public int count;
}

[System.Serializable]
public class WaveDefinition
{
    [Header("Wave settings")]
    public List<WaveDefinitionElement> definition = new List<WaveDefinitionElement>();
}

public class WaveManager : MonoBehaviour
{

    [Header("Waves definition")]
    public List<WaveDefinition> waves = new List<WaveDefinition>();

    [Header("Enemies amount per wave")]
    public int enemiesPerWave;

    [Header("How often enemies add")]
    public int waveChangeRate;

    public GameManager gameManager;

    private int currentWave;

    private List<Enemy> enemyTypesInWaves = new List<Enemy>();

    public int GetCurrentWave() => currentWave;

    public int IncrementWave() => currentWave++;

    public WaveDefinition GetCurrentWaveDefinition() => waves[currentWave - 1];

    public void InfiniteWave()
    {
        if (currentWave == 1) enemyTypesInWaves.Add(gameManager.enemiesPrefabs[0].GetComponent<Enemy>());
        // Every waveChangeRate-th wave, we want to add new enemy type to the waves content
        if (currentWave % waveChangeRate == 0)
        {
            if ((currentWave / waveChangeRate) < gameManager.enemiesPrefabs.Count)
            {
                int enemyIndex = (currentWave / waveChangeRate);
                enemyTypesInWaves.Add(gameManager.enemiesPrefabs[enemyIndex].GetComponent<Enemy>());
            }
        }

        for (int i = 0; i < enemyTypesInWaves.Count; i++)
        {
            List<Enemy> enemiesInWave = new List<Enemy>();
            // The higher the index, the more powerful the enemy
            // Number of spawns is determined by the wave number and divided by the enemy index
            int spawns = currentWave / (i + 1) * enemiesPerWave;
            for (int j = 0; j < spawns; j++)
            {
                enemiesInWave.Add(enemyTypesInWaves[i]);
            }

            StartCoroutine(gameManager.SpawnWave(enemiesInWave));
        }

    }

}
