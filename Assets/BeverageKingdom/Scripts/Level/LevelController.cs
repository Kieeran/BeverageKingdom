using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    MileStone _mileStoneProgressBar;

    [HideInInspector]
    public List<LevelData> LevelDatas;
    LevelData _currentLevelData;

    private int currentWaveIndex = 0;
    private float timer = 0f;

    bool _isLevelComplete;
    bool _isSpawnAllEnemies;
    float _levelDuration;

    private LevelConfiguration levelConfig;
    private int totalLevels = 10; // Default fallback value

    private void Awake()
    {
        Instance = this;
        // Load the level configuration
        levelConfig = Resources.Load<LevelConfiguration>("Levels/LevelConfig");
        if (levelConfig != null)
        {
            totalLevels = levelConfig.TotalLevels;
        }
    }    void Start()
    {
        LoadLevelData();
        int currentLevelIndex = Controller.Instance.CurrentLevelIndex;
        Debug.Log($"Starting Level {currentLevelIndex + 1}");
        if (currentLevelIndex < LevelDatas.Count)
        {
            _currentLevelData = LevelDatas[currentLevelIndex];
            InitLevel();
            UIManager.Instance.PlayCanvas.UpdateLevelText(currentLevelIndex + 1);
        }
        else
        {
            Debug.LogError($"Level {currentLevelIndex + 1} not found!");
        }
    }

    void LoadLevelData()
    {
        LevelDatas = new List<LevelData>();
        bool anyLevelsFound = false;

        for (int i = 1; i <= totalLevels; i++)
        {
            string levelPath = $"Levels/Level{i}/LevelData";
            LevelData levelData = Resources.Load<LevelData>(levelPath);

            if (levelData != null)
            {
                LevelDatas.Add(levelData);
                anyLevelsFound = true;
                Debug.Log($"Successfully loaded {levelPath}");
            }
            else
            {
                Debug.LogError($"Could not load {levelPath}. Make sure to generate levels using the Level Data Helper tool (Tools > Level Data Helper).");
            }
        }

        if (!anyLevelsFound)
        {
            Debug.LogError("No level data found! Please generate levels using Tools > Level Data Helper in the Unity Editor.");
        }
    }    void InitLevel()
    {
        // Reset all level state
        _levelDuration = _currentLevelData.Waves[^1].StartTime;
        currentWaveIndex = 0;
        timer = 0f;
        _isLevelComplete = false;
        _isSpawnAllEnemies = false;

        Debug.Log($"Initializing Level {Controller.Instance.CurrentLevelIndex + 1} with {_currentLevelData.Waves.Count} waves. Total duration: {_levelDuration}s");
        Debug.Log($"Level state reset - Wave Index: {currentWaveIndex}, Timer: {timer}, Complete: {_isLevelComplete}, AllSpawned: {_isSpawnAllEnemies}");

        _mileStoneProgressBar = UIManager.Instance.PlayCanvas.MileStoneProgressBar;

        InitMarker();
    }

    void InitMarker()
    {
        _mileStoneProgressBar.ClearTimeMarker();

        for (int i = 1; i < _currentLevelData.Waves.Count; i++)
        {
            RectTransform markerRect = _mileStoneProgressBar.PlaceTimeMarker(_currentLevelData.Waves[i].StartTime, _levelDuration);

            if (i == _currentLevelData.Waves.Count - 1)
            {
                _mileStoneProgressBar.UpsizeMarker(markerRect, 50f);
            }
        }
    }    void Update()
    {
        // Only check for level completion if all enemies have been spawned and there are none left
        if (_isSpawnAllEnemies)
        {
            bool hasEnemies = EnemySpawner.Instance.IsAnyEnemyInContainer();
            Debug.Log($"Level {Controller.Instance.CurrentLevelIndex + 1} completion check - Enemies remaining: {(hasEnemies ? "Yes" : "No")}, All waves spawned: {_isSpawnAllEnemies}, Level complete: {_isLevelComplete}");
            
            if (!hasEnemies && !_isLevelComplete)
            {
                _isLevelComplete = true;
                Debug.Log($"Level {Controller.Instance.CurrentLevelIndex + 1} completed. All waves done and enemies defeated. Triggering GameWin.");
                GameSystem.instance.GameWin();
                return;
            }
        }

        if (currentWaveIndex >= _currentLevelData.Waves.Count)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer <= _levelDuration)
        {
            UIManager.Instance.PlayCanvas.UpdateLevelProgressBar(timer / _levelDuration);
        }        WaveData wave = _currentLevelData.Waves[currentWaveIndex];        if (timer >= wave.StartTime)
        {
            Debug.Log($"Level {Controller.Instance.CurrentLevelIndex + 1}, Starting Wave {currentWaveIndex + 1}/{_currentLevelData.Waves.Count} at time {timer:F1}s");
            _mileStoneProgressBar.UpdateCompleteMileStone(currentWaveIndex);
            StartCoroutine(EnemySpawner.Instance.SpawnWave(
                wave,
                () =>
                {
                    Debug.Log($"Level {Controller.Instance.CurrentLevelIndex + 1}: Wave {currentWaveIndex + 1} spawn complete");
                    if (currentWaveIndex >= _currentLevelData.Waves.Count)
                    {
                        _isSpawnAllEnemies = true;
                        Debug.Log($"Level {Controller.Instance.CurrentLevelIndex + 1}: All waves completed. Waiting for remaining enemies to be defeated.");
                    }
                }
            ));
            currentWaveIndex++;
        }
    }
}