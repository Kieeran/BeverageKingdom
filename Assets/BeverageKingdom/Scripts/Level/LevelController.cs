using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;

    public List<GameObject> EnemyPrefab;

    MileStone _mileStoneProgressBar;

    public LevelData levelData;
    private int currentWaveIndex = 0;
    private float timer = 0f;
    // private bool isSpawningWave = false;

    List<SpawnArea> _spawnAreas = new();
    bool _isLevelComplete;
    bool _isSpawnAllEnemies;
    float _levelDuration;

    private Coroutine spawnCoroutine;

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

        _mileStoneProgressBar = UIManager.Instance.PlayCanvas.MileStoneProgressBar;

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
            UIManager.Instance.PlayCanvas.UpdateLevelProgressBar(timer / _levelDuration);
        }

        WaveData wave = levelData.Waves[currentWaveIndex];

        if (timer >= wave.StartTime)
        {
            // if (spawnCoroutine != null)
            // {
            //     StopCoroutine(spawnCoroutine);
            //     spawnCoroutine = null;
            // }

            _mileStoneProgressBar.UpdateCompleteMileStone(currentWaveIndex);
            spawnCoroutine = StartCoroutine(SpawnWave(wave, currentWaveIndex));
            currentWaveIndex++;
        }
    }

    bool CheckCompleteLevel()
    {
        return transform.childCount == 0;
    }

    int _groupsRemaining;
    IEnumerator SpawnWave(WaveData wave, int waveIndex)
    {
        // 1) Khởi tạo counter = số nhóm trong wave
        _groupsRemaining = wave.EnemiesToSpawn.Count;

        // 2) Với mỗi spawnData, start 1 coroutine con
        foreach (var spawnData in wave.EnemiesToSpawn)
        {
            StartCoroutine(SpawnEnemyGroup(spawnData));
        }

        // 3) Chờ cho đến khi tất cả nhóm xong (counter về 0)
        yield return new WaitUntil(() => _groupsRemaining == 0);

        // 4) Wave này đã xong
        if (waveIndex == levelData.Waves.Count - 1)
        {
            _isSpawnAllEnemies = true;
            Debug.Log("completed.");
        }
    }

    // Coroutine con chỉ lo spawn nhóm đó
    private IEnumerator SpawnEnemyGroup(EnemySpawnData spawnData)
    {
        for (int i = 0; i < spawnData.Count; i++)
        {
            SpawnEnemy(spawnData.EnemyType);
            yield return new WaitForSeconds(spawnData.SpawnInterval);
        }

        // Khi group này xong, giảm counter
        _groupsRemaining--;
    }

    private void SpawnEnemy(string enemyName)
    {
        GameObject enemyPrefab = GetEnemyPrefab(enemyName);
        if (enemyPrefab == null)
        {
            Debug.LogError($"Enemy prefab not found: {enemyName}");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, _spawnAreas[Random.Range(0, 3)].GetRandomSpawnPos(), Quaternion.identity);
        enemy.transform.SetParent(transform);
    }

    public Transform GetRadomEnemy()
    {
        int random = Random.Range(0, transform.childCount);
        Transform enemy = transform.GetChild(random);
        return enemy;
    }

    private GameObject GetEnemyPrefab(string name)
    {
        foreach (var enemy in EnemyPrefab)
        {
            if (enemy.name == name)
            {
                return enemy;
            }
        }
        return null;
    }
}