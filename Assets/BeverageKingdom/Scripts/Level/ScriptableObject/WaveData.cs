using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Game Data/Wave Data")]
public class WaveData : ScriptableObject
{
    public float StartTime;
    public List<EnemySpawnData> EnemiesToSpawn;
    public List<HotSpotSpawnData> HotSpotsToSpawn;
}