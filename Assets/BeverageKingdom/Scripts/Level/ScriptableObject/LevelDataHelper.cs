using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq; // Added for Sum()

#if UNITY_EDITOR

// Helper class to define enemy type and its share in a wave
[System.Serializable]
public class EnemyShare
{
    public string enemyType = "Warrior";
    public float share = 1.0f;
}

public class LevelDataHelper : EditorWindow
{
    private static float baseEnemyCount = 4;  // Reduced initial count for better early game
    private static float enemyIncreasePerLevel = 2;
    private static float baseSpawnInterval = 2.0f;  // Increased for better pacing
    private static float spawnIntervalDecreasePerLevel = 0.15f;
    private static float waveStartTimeInterval = 25f;  // Increased to give more time between waves
    private static string levelsPath = "Assets/BeverageKingdom/Resources/Levels";
    private static int numberOfLevels = 10; // Default number of levels

    // New settings for easy levels
    private static int easyLevelFrequency = 3; // e.g., every 3 regular levels, the next is easy (4th, 8th...)
    private static float easyLevelEnemyCountMultiplier = 0.7f; // 70% of normal enemies
    private static float easyLevelSpawnIntervalMultiplier = 1.3f; // 30% longer spawn intervals

    // Replaces the old waveEnemyTypes
    private List<List<EnemyShare>> wavesEnemyShares = new List<List<EnemyShare>>();
    private int numberOfConfigurableWaves = 4; // Default, will be updated by wavesEnemyShares.Count

    // Comment out or remove the old static waveEnemyTypes
    // private static readonly (string type, float ratio)[][] waveEnemyTypes = new[]
    // {   
    //     // ... old data ...
    // };

    [MenuItem("Tools/Level Data Helper")]
    public static void ShowWindow()
    {
        GetWindow<LevelDataHelper>("Level Data Helper");
    }

    void OnEnable()
    {
        // wavesEnemyShares and numberOfConfigurableWaves are instance fields and should be serialized by Unity EditorWindow.
        // Initialize with defaults only if wavesEnemyShares is null or empty,
        // which might happen on first creation or if serialization was lost/reset.
        if (wavesEnemyShares == null || wavesEnemyShares.Count == 0)
        {
            InitializeWaveEnemyShares(); // This will set both wavesEnemyShares and numberOfConfigurableWaves
        }
        else
        {
            // If wavesEnemyShares is populated, it means Unity deserialized it.
            // Ensure numberOfConfigurableWaves is consistent with the loaded data.
            // This also handles the case where the list is not null but numberOfConfigurableWaves might be out of sync.
            numberOfConfigurableWaves = wavesEnemyShares.Count;
        }
    }

    void InitializeWaveEnemyShares()
    {
        // This method now strictly initializes to default values.
        // It's called by OnEnable if data is missing.
        wavesEnemyShares = new List<List<EnemyShare>>();
        
        // Default wave configurations (similar to original, but adjustable)
        // Wave 1: Only Warriors
        wavesEnemyShares.Add(new List<EnemyShare> { new EnemyShare { enemyType = "Warrior", share = 1.0f } });
        // Wave 2: Warriors and Scouts
        wavesEnemyShares.Add(new List<EnemyShare> { 
            new EnemyShare { enemyType = "Warrior", share = 0.8f }, 
            new EnemyShare { enemyType = "Scout", share = 0.2f } 
        });
        // Wave 3: Scouts, Warriors, and Heavies
        wavesEnemyShares.Add(new List<EnemyShare> { 
            new EnemyShare { enemyType = "Scout", share = 0.45f }, 
            new EnemyShare { enemyType = "Warrior", share = 0.45f }, 
            new EnemyShare { enemyType = "Heavy", share = 0.1f } 
        });
        // Wave 4: Scouts, Warriors, Heavies, and a Boss
        wavesEnemyShares.Add(new List<EnemyShare> { 
            new EnemyShare { enemyType = "Scout", share = 0.3f }, 
            new EnemyShare { enemyType = "Warrior", share = 0.3f }, 
            new EnemyShare { enemyType = "Heavy", share = 0.3f }, 
            new EnemyShare { enemyType = "Boss", share = 0.1f } 
        });
        numberOfConfigurableWaves = wavesEnemyShares.Count; // Sync this after initializing
    }

    void OnGUI()
    {
        GUILayout.Label("Level Configuration", EditorStyles.boldLabel);
        
        numberOfLevels = EditorGUILayout.IntField("Number of Levels", numberOfLevels);
        baseEnemyCount = EditorGUILayout.FloatField("Base Enemy Count", baseEnemyCount);
        enemyIncreasePerLevel = EditorGUILayout.FloatField("Enemy Increase Per Level", enemyIncreasePerLevel);
        baseSpawnInterval = EditorGUILayout.FloatField("Base Spawn Interval", baseSpawnInterval);
        spawnIntervalDecreasePerLevel = EditorGUILayout.FloatField("Spawn Interval Decrease", spawnIntervalDecreasePerLevel);
        waveStartTimeInterval = EditorGUILayout.FloatField("Wave Time Interval", waveStartTimeInterval);

        EditorGUILayout.Space();
        GUILayout.Label("Easy Level Configuration", EditorStyles.boldLabel);
        easyLevelFrequency = EditorGUILayout.IntField(new GUIContent("Easy Level Every X Levels", "An easy level will appear after this many regular levels. E.g., 3 means 4th, 8th... are easy."), easyLevelFrequency);
        if (easyLevelFrequency < 1) easyLevelFrequency = 1;
        easyLevelEnemyCountMultiplier = EditorGUILayout.Slider(new GUIContent("Easy Lvl Enemy Multiplier", "Multiplier for enemy count on easy levels."), easyLevelEnemyCountMultiplier, 0.1f, 1.0f);
        easyLevelSpawnIntervalMultiplier = EditorGUILayout.Slider(new GUIContent("Easy Lvl Spawn Multiplier", "Multiplier for spawn interval on easy levels."), easyLevelSpawnIntervalMultiplier, 1.0f, 3.0f);

        EditorGUILayout.Space();
        GUILayout.Label("Wave Enemy Configuration", EditorStyles.boldLabel);
        
        int newNumberOfWaves = EditorGUILayout.IntField("Number of Waves Per Level", numberOfConfigurableWaves);
        if (newNumberOfWaves != numberOfConfigurableWaves)
        {
            numberOfConfigurableWaves = Mathf.Max(1, newNumberOfWaves); // Ensure at least 1 wave
            // Adjust list size
            while (wavesEnemyShares.Count < numberOfConfigurableWaves)
                wavesEnemyShares.Add(new List<EnemyShare> { new EnemyShare { enemyType = "Warrior", share = 1.0f } }); // Add new default wave
            while (wavesEnemyShares.Count > numberOfConfigurableWaves)
                wavesEnemyShares.RemoveAt(wavesEnemyShares.Count - 1); // Remove excess waves
            GUI.changed = true; // Mark GUI as changed
        }

        // if (wavesEnemyShares == null) InitializeWaveEnemyShares(); // This check is now primarily handled by OnEnable

        for (int i = 0; i < wavesEnemyShares.Count; i++)
        {
            GUILayout.Label($"Wave {i + 1} Distribution", EditorStyles.boldLabel);
            var waveShareList = wavesEnemyShares[i];
            
            if (GUILayout.Button($"+ Add Enemy Type to Wave {i + 1}"))
            {
                waveShareList.Add(new EnemyShare { enemyType = "NewEnemy", share = 0.0f});
                GUI.changed = true;
            }

            float currentTotalShare = 0f;
            for (int j = 0; j < waveShareList.Count; )
            {
                EditorGUILayout.BeginHorizontal();
                waveShareList[j].enemyType = EditorGUILayout.TextField("Type", waveShareList[j].enemyType);
                waveShareList[j].share = EditorGUILayout.FloatField("Share", waveShareList[j].share);
                waveShareList[j].share = Mathf.Clamp01(waveShareList[j].share); // Ensure 0-1
                
                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    waveShareList.RemoveAt(j);
                    EditorGUILayout.EndHorizontal();
                    GUI.changed = true; 
                    break; // Exit loop to re-render, avoids index issues
                }
                currentTotalShare += waveShareList[j].share; // Sum after potential removal check
                EditorGUILayout.EndHorizontal();
                j++; // Increment only if not removed
            }

            if (Mathf.Abs(currentTotalShare - 1.0f) > 0.01f && waveShareList.Count > 0) {
                EditorGUILayout.HelpBox($"Shares for Wave {i+1} do not sum to 1.0 (Current sum: {currentTotalShare:F2}). Please adjust.", MessageType.Warning);
            } else if (waveShareList.Count == 0) {
                EditorGUILayout.HelpBox($"Wave {i+1} has no enemy types defined.", MessageType.Warning);
            }
            EditorGUILayout.Space();
        }


        // Add min/max validation for number of levels
        if (numberOfLevels < 1)
        {
            EditorGUILayout.HelpBox("Number of levels must be at least 1", MessageType.Warning);
        }

        GUI.enabled = numberOfLevels >= 1;
        if (GUILayout.Button("Generate All Levels"))
        {
            GenerateAllLevels();
        }
        GUI.enabled = true;
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
                // Debug.Log($"Deleted existing asset: {file}"); // Can be verbose
            }
        }
        Debug.Log("Cleared existing level data.");

        // Create a configuration file to store the number of levels
        var config = CreateInstance<LevelConfiguration>();
        config.TotalLevels = numberOfLevels;
        SaveAsset(config, $"{levelsPath}/LevelConfig.asset");
        Debug.Log($"Saved level configuration with {numberOfLevels} levels");

        for (int levelIdx = 0; levelIdx < numberOfLevels; levelIdx++) // Renamed to levelIdx to avoid conflict
        {
            int currentLevelNumber = levelIdx + 1;
            Debug.Log($"Generating Level {currentLevelNumber}...");
            
            string levelFolderPath = $"{levelsPath}/Level{currentLevelNumber}";
            EnsureDirectoryExists(levelFolderPath);

            LevelData levelData = CreateInstance<LevelData>();
            levelData.Waves = new List<WaveData>();

            // Determine if this is an easy level
            // (levelIdx + 1) makes it 1-based for modulo. If easyLevelFrequency is 3, levels 4, 8, 12... are easy.
            bool isEasyLevel = (easyLevelFrequency > 0 && currentLevelNumber % (easyLevelFrequency + 1) == 0) ;


            // Calculate difficulty parameters for this level
            float currentBaseEnemyCountForLevel = baseEnemyCount + (levelIdx * enemyIncreasePerLevel);
            float currentSpawnIntervalForLevel = Mathf.Max(0.5f, baseSpawnInterval - (levelIdx * spawnIntervalDecreasePerLevel));

            if (isEasyLevel)
            {
                Debug.Log($"Level {currentLevelNumber} is an EASY level!");
                currentBaseEnemyCountForLevel *= easyLevelEnemyCountMultiplier;
                currentSpawnIntervalForLevel *= easyLevelSpawnIntervalMultiplier;
                currentSpawnIntervalForLevel = Mathf.Max(0.2f, currentSpawnIntervalForLevel); // Ensure interval doesn't get too short
            }


            // Create waves per level based on wavesEnemyShares configuration
            for (int waveIdx = 0; waveIdx < wavesEnemyShares.Count; waveIdx++) // Renamed to waveIdx
            {
                Debug.Log($"Generating Wave {waveIdx + 1} for Level {currentLevelNumber}");
                
                WaveData waveData = CreateInstance<WaveData>();
                waveData.StartTime = waveIdx * waveStartTimeInterval;
                waveData.EnemiesToSpawn = new List<EnemySpawnData>();

                // Define waveDataPath once per wave iteration, before any potential 'continue'
                string waveDataPath = $"{levelFolderPath}/Wave{waveIdx + 1}.asset";

                // Calculate total enemies for this wave (including wave scaling)
                float waveMultiplier = 1 + (waveIdx * 0.5f); // Wave 1: 1x, Wave 2: 1.5x, etc.
                int totalEnemiesInWave = Mathf.Max(1, Mathf.RoundToInt(currentBaseEnemyCountForLevel * waveMultiplier)); // Ensure at least 1 enemy if base count is low

                var currentWaveShares = wavesEnemyShares[waveIdx];
                if (currentWaveShares == null || currentWaveShares.Count == 0)
                {
                    Debug.LogWarning($"Wave {waveIdx + 1} for Level {currentLevelNumber} has no enemy shares defined. Skipping enemy generation for this wave.");
                    // Save wave data (it will be empty)
                    SaveAsset(waveData, waveDataPath);
                    levelData.Waves.Add(waveData);
                    continue;
                }
                
                // Distribute enemies according to type ratios from wavesEnemyShares
                foreach (var enemyShare in currentWaveShares)
                {
                    if (string.IsNullOrEmpty(enemyShare.enemyType) || enemyShare.share <= 0)
                    {
                        continue; // Skip if type is not set or share is zero/negative
                    }

                    int enemyCountForType = Mathf.RoundToInt(totalEnemiesInWave * enemyShare.share);
                    if (enemyCountForType > 0)
                    {
                        EnemySpawnData enemySpawn = CreateInstance<EnemySpawnData>();
                        enemySpawn.Count = enemyCountForType;
                        enemySpawn.SpawnInterval = currentSpawnIntervalForLevel; // Use level-specific (and potentially easy-level modified) interval
                        enemySpawn.EnemyType = enemyShare.enemyType;

                        string spawnDataPath = $"{levelFolderPath}/Wave{waveIdx + 1}_{enemyShare.enemyType}Spawn.asset";
                        SaveAsset(enemySpawn, spawnDataPath);
                        // Debug.Log($"Created {enemyShare.enemyType} spawn data: Count={enemyCountForType}, Interval={currentSpawnIntervalForLevel:F2}s");

                        waveData.EnemiesToSpawn.Add(enemySpawn);
                    }
                }
                // Log total enemies actually assigned to ensure sum of shares logic is working
                int actualSpawnedCount = waveData.EnemiesToSpawn.Sum(e => e.Count);
                if (Mathf.Abs(currentWaveShares.Sum(s => s.share) - 1.0f) < 0.01f && actualSpawnedCount != totalEnemiesInWave && totalEnemiesInWave > 0)
                {
                     Debug.LogWarning($"Wave {waveIdx + 1} for Level {currentLevelNumber}: Target enemies {totalEnemiesInWave}, actual from shares {actualSpawnedCount}. Check rounding or share sum.");
                }


                // Save wave data
                SaveAsset(waveData, waveDataPath); // Use waveDataPath defined above
                // Debug.Log($"Saved Wave {waveIdx + 1} data");

                levelData.Waves.Add(waveData);
            }

            // Save level data
            string levelDataPath = $"{levelFolderPath}/LevelData.asset";
            SaveAsset(levelData, levelDataPath);
            Debug.Log($"Completed Level {currentLevelNumber} generation");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Successfully generated all level data!");
    }
}
#endif
