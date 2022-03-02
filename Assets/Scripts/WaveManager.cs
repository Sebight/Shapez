using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int currentWave;

    public int GetCurrentWave => currentWave;

    public int IncrementWave() => currentWave++;
}
