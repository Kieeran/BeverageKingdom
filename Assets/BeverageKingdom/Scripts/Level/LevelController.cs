using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

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

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadLevelData();
        int currentLevelIndex = Controller.Instance.CurrentLevelIndex;
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

        for (int i = 1; i <= 10; i++)
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
    }

    void InitLevel()
    {
        _levelDuration = _currentLevelData.Waves[^1].StartTime;

        _isLevelComplete = false;
        _isSpawnAllEnemies = false;

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
    }

    void Update()
    {
        if (!EnemySpawner.Instance.IsAnyEnemyInContainer() && _isLevelComplete == false && _isSpawnAllEnemies == true)
        {
            _isLevelComplete = true;
            GameSystem.instance.GameWin();
        }

        if (currentWaveIndex >= _currentLevelData.Waves.Count)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer <= _levelDuration)
        {
            UIManager.Instance.PlayCanvas.UpdateLevelProgressBar(timer / _levelDuration);
        }

        WaveData wave = _currentLevelData.Waves[currentWaveIndex];

        if (timer >= wave.StartTime)
        {
            _mileStoneProgressBar.UpdateCompleteMileStone(currentWaveIndex);
            StartCoroutine(EnemySpawner.Instance.SpawnWave(
                wave,
                () =>
                {
                    if (currentWaveIndex == _currentLevelData.Waves.Count - 1)
                    {
                        _isSpawnAllEnemies = true;
                        Debug.Log("completed.");
                    }
                }
            ));
            currentWaveIndex++;
        }
    }
}