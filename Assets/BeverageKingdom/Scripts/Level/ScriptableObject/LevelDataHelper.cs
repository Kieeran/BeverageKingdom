using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
public class LevelDataHelper : EditorWindow
{
    private static float baseEnemyCount = 5;
    private static float enemyIncreasePerLevel = 2;
    private static float baseSpawnInterval = 1.5f;
    private static float spawnIntervalDecreasePerLevel = 0.1f;
    private static float waveStartTimeInterval = 20f;
    private static string levelsPath = "Assets/BeverageKingdom/Resources/Levels";

    [MenuItem("Tools/Level Data Helper")]
    public static void ShowWindow()
    {
        GetWindow<LevelDataHelper>("Level Data Helper");
    }

    void OnGUI()
    {
        GUILayout.Label("Level Configuration", EditorStyles.boldLabel);
        
        baseEnemyCount = EditorGUILayout.FloatField("Base Enemy Count", baseEnemyCount);
        enemyIncreasePerLevel = EditorGUILayout.FloatField("Enemy Increase Per Level", enemyIncreasePerLevel);
        baseSpawnInterval = EditorGUILayout.FloatField("Base Spawn Interval", baseSpawnInterval);
        spawnIntervalDecreasePerLevel = EditorGUILayout.FloatField("Spawn Interval Decrease", spawnIntervalDecreasePerLevel);
        waveStartTimeInterval = EditorGUILayout.FloatField("Wave Time Interval", waveStartTimeInterval);

        if (GUILayout.Button("Generate All Levels"))
        {
            GenerateAllLevels();
        }
    }

    private void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
    }

    private void SaveAsset<T>(T asset, string path) where T : ScriptableObject
    {
        AssetDatabase.CreateAsset(asset, path);
        EditorUtility.SetDirty(asset);
    }

    private void GenerateAllLevels()
    {
        // Ensure the Levels directory exists
        EnsureDirectoryExists(levelsPath);

        // Clear existing level data
        if (Directory.Exists(levelsPath))
        {
            string[] files = Directory.GetFiles(levelsPath, "*.asset");
            foreach (string file in files)
            {
                AssetDatabase.DeleteAsset(file);
            }
        }

        for (int level = 0; level < 10; level++)
        {
            string levelFolderPath = $"{levelsPath}/Level{level + 1}";
            EnsureDirectoryExists(levelFolderPath);

            LevelData levelData = CreateInstance<LevelData>();
            levelData.Waves = new List<WaveData>();

            // Calculate difficulty parameters for this level
            float enemyCount = baseEnemyCount + (level * enemyIncreasePerLevel);
            float spawnInterval = Mathf.Max(0.5f, baseSpawnInterval - (level * spawnIntervalDecreasePerLevel));

            // Create 3 waves per level
            for (int wave = 0; wave < 3; wave++)
            {
                WaveData waveData = CreateInstance<WaveData>();
                waveData.StartTime = wave * waveStartTimeInterval;
                waveData.IsBossWave = (wave == 2); // Last wave is boss wave
                waveData.EnemiesToSpawn = new List<EnemySpawnData>();

                // Add enemies to wave
                EnemySpawnData enemySpawn = CreateInstance<EnemySpawnData>();
                enemySpawn.Count = Mathf.RoundToInt(enemyCount * (wave + 1));
                enemySpawn.SpawnInterval = spawnInterval;
                enemySpawn.EnemyType = waveData.IsBossWave ? "Boss" : "Normal";

                // Save enemy spawn data first
                string enemySpawnPath = $"{levelFolderPath}/EnemySpawn_L{level + 1}_W{wave + 1}.asset";
                SaveAsset(enemySpawn, enemySpawnPath);

                // Add to wave and save wave data
                waveData.EnemiesToSpawn.Add(enemySpawn);
                string wavePath = $"{levelFolderPath}/Wave_L{level + 1}_W{wave + 1}.asset";
                SaveAsset(waveData, wavePath);

                levelData.Waves.Add(waveData);
            }

            // Save level data
            string levelPath = $"{levelsPath}/Level_{level + 1}.asset";
            SaveAsset(levelData, levelPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Generated all level data!");
    }
}
#endif
