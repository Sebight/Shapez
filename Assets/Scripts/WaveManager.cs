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

    public GameManager gameManager;

    private int currentWave;

    public int GetCurrentWave() => currentWave;

    public int IncrementWave() => currentWave++;

    public WaveDefinition GetCurrentWaveDefinition() => waves[currentWave-1];
}
