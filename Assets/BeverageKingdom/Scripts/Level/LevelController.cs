using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;

    public GameObject EnemyPrefab;

    MileStone _mileStoneProgressBar;

<<<<<<< Updated upstream
    public LevelData levelData;
=======
    [HideInInspector]
    public List<LevelData> LevelDatas;
    LevelData _currentLevelData;

>>>>>>> Stashed changes
    private int currentWaveIndex = 0;
    private float timer = 0f;
    // private bool isSpawningWave = false;

    List<SpawnArea> _spawnAreas = new();
    bool _isLevelComplete;
    bool _isSpawnAllEnemies;
    float _levelDuration;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
<<<<<<< Updated upstream
        Env env = Controller.Instance.Env.GetComponent<Env>();
=======
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
        for (int i = 1; i <= 10; i++)
        {
            LevelData levelData = Resources.Load<LevelData>($"Levels/Level_{i}");
            if (levelData != null)
            {
                LevelDatas.Add(levelData);
            }
            else
            {
                Debug.LogWarning($"Could not load Level_{i}. Make sure to generate levels using the Level Data Helper tool.");
            }
        }
    }
>>>>>>> Stashed changes

        _spawnAreas.Add(env.EnemySpawnPosSlot1.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.EnemySpawnPosSlot2.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.EnemySpawnPosSlot3.GetChild(0).GetComponent<SpawnArea>());

        _levelDuration = levelData.Waves[levelData.Waves.Count - 1].StartTime;

        _isLevelComplete = false;
        _isSpawnAllEnemies = false;

        _mileStoneProgressBar = UIManager.Instance.MainCanvas.MileStoneProgressBar;

        InitMarker();
    }

    void InitMarker()
    {
        for (int i = 1; i < levelData.Waves.Count; i++)
        {
            RectTransform markerRect = _mileStoneProgressBar.PlaceTimeMarker(levelData.Waves[i].StartTime, _levelDuration);

            if (i == levelData.Waves.Count - 1)
            {
                _mileStoneProgressBar.UpsizeMarker(markerRect, 50f);
            }
        }
    }

    void Update()
    {
        if (CheckCompleteLevel() && _isLevelComplete == false && _isSpawnAllEnemies == true)
        {
            _isLevelComplete = true;
            GameSystem.instance.GameWin();
        }

        if (currentWaveIndex >= levelData.Waves.Count)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer <= _levelDuration)
        {
            UIManager.Instance.MainCanvas.UpdateLevelProgressBar(timer / _levelDuration);
        }
        // Debug.Log(timer + "  " + levelData.Waves[currentWaveIndex].StartTime);

        WaveData wave = levelData.Waves[currentWaveIndex];
        // Debug.Log("Wave" + (currentWaveIndex + 1));

        if (/*!isSpawningWave &&*/ timer >= wave.StartTime)
        {
            _mileStoneProgressBar.UpdateCompleteMileStone(currentWaveIndex);
            StartCoroutine(SpawnWave(wave, currentWaveIndex));
            currentWaveIndex++;
        }
    }

    bool CheckCompleteLevel()
    {
        return transform.childCount == 0;
    }

    IEnumerator SpawnWave(WaveData wave, int waveIndex)
    {
        foreach (var spawnData in wave.EnemiesToSpawn)
        {
            for (int i = 0; i < spawnData.Count; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnData.SpawnInterval);
            }
        }

        if (waveIndex == levelData.Waves.Count - 1)
        {
            _isSpawnAllEnemies = true;
        }

        // currentWaveIndex++;
        // isSpawningWave = false;
    }

    void SpawnEnemy()
    {
        int laneIndex = Random.Range(0, _spawnAreas.Count);
        Vector2 spawnPos = _spawnAreas[laneIndex].GetRandomSpawnPos();
        GameObject enemy = Instantiate(EnemyPrefab, spawnPos, Quaternion.identity);
        enemy.transform.SetParent(transform);
    }
    public Transform GetRadomEnemy()
    {
        int random = Random.Range(0, transform.childCount);
        Transform enemy = transform.GetChild(random);
        return enemy;
    }
}