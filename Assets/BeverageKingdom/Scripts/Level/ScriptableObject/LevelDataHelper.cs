using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
public class LevelDataHelper : EditorWindow
{
    private static float baseEnemyCount = 4;  // Reduced initial count for better early game
    private static float enemyIncreasePerLevel = 2;
    private static float baseSpawnInterval = 2.0f;  // Increased for better pacing
    private static float spawnIntervalDecreasePerLevel = 0.15f;
    private static float waveStartTimeInterval = 25f;  // Increased to give more time between waves
    private static string levelsPath = "Assets/BeverageKingdom/Resources/Levels";

    // Enemy type distribution per wave - adjusted for better progression
    private static readonly (string type, float ratio)[][] waveEnemyTypes = new[]
    {
        // Wave 1: Mostly scouts with few warriors (early game wave)
        new[] { ("Scout", 0.8f), ("Warrior", 0.2f) },
        // Wave 2: Balanced mix (mid wave)
        new[] { ("Scout", 0.4f), ("Warrior", 0.4f), ("Heavy", 0.2f) },
        // Wave 3: Challenging wave with tough enemies
        new[] { ("Scout", 0.2f), ("Warrior", 0.5f), ("Heavy", 0.3f) }
    };

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
            Debug.Log($"Created directory: {path}");
            AssetDatabase.Refresh();
        }
    }

    private void SaveAsset<T>(T asset, string path) where T : ScriptableObject
    {
        try
        {
            // Make sure the directory exists
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                EnsureDirectoryExists(directory);
            }

            // Create or update the asset
            AssetDatabase.CreateAsset(asset, path);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            
            Debug.Log($"Successfully saved asset: {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving asset to {path}: {e.Message}");
        }
    }

    private void GenerateAllLevels()
    {
        Debug.Log("Starting level generation...");

        // Ensure the Resources directory exists
        string resourcesPath = "Assets/BeverageKingdom/Resources";
        EnsureDirectoryExists(resourcesPath);
        
        // Ensure the Levels directory exists
        EnsureDirectoryExists(levelsPath);

        // Clear existing level data
        if (Directory.Exists(levelsPath))
        {
            string[] files = Directory.GetFiles(levelsPath, "*.asset", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                AssetDatabase.DeleteAsset(file);
                Debug.Log($"Deleted existing asset: {file}");
            }
        }

        for (int level = 0; level < 10; level++)
        {
            Debug.Log($"Generating Level {level + 1}...");
            
            string levelFolderPath = $"{levelsPath}/Level{level + 1}";
            EnsureDirectoryExists(levelFolderPath);

            LevelData levelData = CreateInstance<LevelData>();
            levelData.Waves = new List<WaveData>();

            // Calculate difficulty parameters for this level
            float baseCount = baseEnemyCount + (level * enemyIncreasePerLevel);
            float spawnInterval = Mathf.Max(0.5f, baseSpawnInterval - (level * spawnIntervalDecreasePerLevel));

            // Create 3 waves per level
            for (int wave = 0; wave < 3; wave++)
            {
                Debug.Log($"Generating Wave {wave + 1} for Level {level + 1}");
                
                WaveData waveData = CreateInstance<WaveData>();
                waveData.StartTime = wave * waveStartTimeInterval;
                waveData.EnemiesToSpawn = new List<EnemySpawnData>();

                // Calculate total enemies for this wave (including wave scaling)
                float waveMultiplier = 1 + (wave * 0.5f); // Wave 1: 1x, Wave 2: 1.5x, Wave 3: 2x
                int totalEnemies = Mathf.RoundToInt(baseCount * waveMultiplier);

                // Distribute enemies according to type ratios
                foreach (var (type, ratio) in waveEnemyTypes[wave])
                {
                    int enemyCount = Mathf.RoundToInt(totalEnemies * ratio);
                    if (enemyCount > 0)
                    {
                        EnemySpawnData enemySpawn = CreateInstance<EnemySpawnData>();
                        enemySpawn.Count = enemyCount;
                        enemySpawn.SpawnInterval = spawnInterval;
                        enemySpawn.EnemyType = type;

                        string spawnDataPath = $"{levelFolderPath}/Wave{wave + 1}_{type}Spawn.asset";
                        SaveAsset(enemySpawn, spawnDataPath);
                        Debug.Log($"Created {type} spawn data: Count={enemyCount}, Interval={spawnInterval:F2}s");

                        waveData.EnemiesToSpawn.Add(enemySpawn);
                    }
                }

                // Save wave data
                string waveDataPath = $"{levelFolderPath}/Wave{wave + 1}.asset";
                SaveAsset(waveData, waveDataPath);
                Debug.Log($"Saved Wave {wave + 1} data");

                levelData.Waves.Add(waveData);
            }

            // Save level data
            string levelDataPath = $"{levelFolderPath}/LevelData.asset";
            SaveAsset(levelData, levelDataPath);
            Debug.Log($"Completed Level {level + 1} generation");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Successfully generated all level data!");
    }
}
#endif
