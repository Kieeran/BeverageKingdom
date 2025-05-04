using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "ScriptableObjects/LevelData", order = 3)]
public class LevelDataSO : ScriptableObject
{
    public WaveDataSO[] waves;   // Danh s�ch c�c wave trong level
}
