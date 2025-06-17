using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnData", menuName = "Game Data/Enemy Spawn Data")]
public class EnemySpawnData : ScriptableObject
{
    public string EnemyType;
    public int Count;
    public float SpawnInterval;
}
