using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyWave
{
    public List<EnemyData> enemies = new List<EnemyData>(); // Danh sách enemy trong wave
}

[System.Serializable]
public class EnemyData
{
    public EnemySO enemyData;   // Dữ liệu cơ bản của enemy
    public int count;              // Số lượng enemy
}

[CreateAssetMenu(fileName = "NewEnemyWaveData", menuName = "ScriptableObjects/EnemyWaveData", order = 1)]
public class WaveDataSO : ScriptableObject
{
    public List<EnemyWave> waves = new List<EnemyWave>();

    [ContextMenu("Add New Wave")]
    public void AddWave()
    {
        EnemyWave newWave = new EnemyWave {  };
        waves.Add(newWave);
    }

    [ContextMenu("Clear All Waves")]
    public void ClearWaves()
    {
        waves.Clear();
    }
}
