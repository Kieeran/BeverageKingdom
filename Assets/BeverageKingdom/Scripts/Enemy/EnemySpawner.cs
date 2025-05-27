using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : Spawner
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Wave Configuration")]
    [Tooltip("ScriptableObject chứa danh sách các wave")]
    public WaveDataSO waveDataSO;

    [Header("Spawn Area")]
    public float spawnX = 10f;
    public float minY = -2f;
    public float maxY = 2f;

    [Header("Spawn Interval (seconds)")]
    public float minSpawnInterval = 0.2f;
    public float maxSpawnInterval = 0.3f;

    // Đếm số enemy đang sống
    private int aliveEnemies;

    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    protected override void Start()
    {
        if (waveDataSO == null || waveDataSO.waves.Count == 0)
        {
            Debug.LogWarning("EnemySpawner: WaveDataSO chưa gán hoặc không có wave nào!");
            return;
        }
        StartCoroutine(RunAllWaves());
    }

    private IEnumerator RunAllWaves()
    {
        for (int i = 0; i < waveDataSO.waves.Count; i++)
        {
            EnemyWave wave = waveDataSO.waves[i];
            Debug.Log($"--- Bắt đầu Wave {i + 1} ---");
            yield return RunSingleWave(wave, i + 1);
        }
        Debug.Log("=== Đã hoàn thành tất cả các wave ===");
        GameSystem.instance.GameWin();
    }

    private IEnumerator RunSingleWave(EnemyWave wave, int waveNumber)
    {
        // Reset counter
        aliveEnemies = 0;

        // Spawn theo từng EnemyData
        foreach (var enemyData in wave.enemies)
        {
            MainCanvas.instance.ShowNextWave(waveNumber, enemyData.count);
            for (int j = 0; j < enemyData.count; j++)
            {
                // Spawn 1 enemy
                Transform t = Spawn(
                    enemyData.enemyData.name,
                    GetRandomSpawnPos(),
                    Quaternion.identity
                );
                if (t != null && t.TryGetComponent<Enemy>(out var e))
                {
                    aliveEnemies++;
                }

                // Chờ random interval trước khi spawn tiếp
                float wait = Random.Range(minSpawnInterval, maxSpawnInterval);
                yield return new WaitForSeconds(wait);
            }
        }

        // Đợi đến khi người chơi kill sạch
        yield return new WaitUntil(() => aliveEnemies == 0);

        Debug.Log($"--- Wave {waveNumber} cleared! ---");
        MainCanvas.instance.ShowAllWavesCompleted();
    }

    /// <summary>
    /// Gọi từ Enemy.Die() để giảm counter aliveEnemies
    /// </summary>
    public void NotifyEnemyKilled()
    {
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
    }

    private Vector3 GetRandomSpawnPos()
    {
        float y = Random.Range(minY, maxY);
        return new Vector3(spawnX, y, 0f);
    }

    public override Transform Spawn(string prefabName, Vector3 spawnPos, Quaternion rotation)
    {
        return base.Spawn(prefabName, spawnPos, rotation);
    }
}
