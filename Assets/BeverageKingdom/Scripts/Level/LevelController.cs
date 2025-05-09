using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;

    public GameObject EnemyPrefab;

    MileStone _mileStoneProgressBar;

    public LevelData levelData;
    private int currentWaveIndex = 0;
    private float timer = 0f;
    private bool isSpawningWave = false;

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
        Env env = Controller.Instance.Env.GetComponent<Env>();

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
        for (int i = 0; i < levelData.Waves.Count; i++)
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
            _isSpawnAllEnemies = true;
            return;
        }

        timer += Time.deltaTime;

        if (timer <= _levelDuration)
        {
            UIManager.Instance.MainCanvas.UpdateLevelProgressBar(timer / _levelDuration);
        }
        Debug.Log(timer + "  " + levelData.Waves[currentWaveIndex].StartTime);

        WaveData wave = levelData.Waves[currentWaveIndex];
        Debug.Log("Wave" + (currentWaveIndex + 1));

        if (!isSpawningWave && timer >= wave.StartTime)
        {
            StartCoroutine(SpawnWave(wave));
            isSpawningWave = true;

            _mileStoneProgressBar.UpdateCompleteMileStone(currentWaveIndex + 1);
        }
    }

    bool CheckCompleteLevel()
    {
        return transform.childCount == 0;
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        foreach (var spawnData in wave.EnemiesToSpawn)
        {
            for (int i = 0; i < spawnData.Count; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnData.SpawnInterval);
            }
        }

        currentWaveIndex++;
        isSpawningWave = false;
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