using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : Spawner
{
    public static EnemySpawner Instance { get; private set; }

<<<<<<< Updated upstream
    [Header("Wave Configuration")]
    [Tooltip("ScriptableObject chứa danh sách các wave")]
    public WaveDataSO waveDataSO;
=======
    [Header("Enemy Prefab References")]
    public GameObject normalEnemyPrefab;
    public GameObject bossEnemyPrefab;

    List<SpawnArea> _spawnAreas = new();
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
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
=======
        Instance = this;
        
        // Initialize the enemy prefabs list if it's empty
        if (EnemyPrefab == null || EnemyPrefab.Count == 0)
        {
            EnemyPrefab = new List<GameObject>();
            if (normalEnemyPrefab != null)
                EnemyPrefab.Add(normalEnemyPrefab);
            if (bossEnemyPrefab != null)
                EnemyPrefab.Add(bossEnemyPrefab);
        }
    }

    void Start()
    {
        Env env = Controller.Instance.Env.GetComponent<Env>();

        _spawnAreas.Add(env.EnemySpawnPosSlot1.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.EnemySpawnPosSlot2.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.EnemySpawnPosSlot3.GetChild(0).GetComponent<SpawnArea>());

        // Validate enemy prefabs
        if (EnemyPrefab.Count == 0)
        {
            Debug.LogError("No enemy prefabs assigned to EnemySpawner! Please assign normal and boss enemy prefabs in the inspector.");
        }
    }

    public void SpawnEnemy(string enemyName)
    {
        GameObject enemyPrefab = GetEnemyPrefab(enemyName);
        if (enemyPrefab == null)
        {
            Debug.LogError($"Enemy prefab not found: {enemyName}. Available prefabs: {string.Join(", ", EnemyPrefab.ConvertAll(p => p.name))}");
            return;
        }

        Vector2 spawnPos = GetRandomSpawnPos();
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy.transform.SetParent(transform);
>>>>>>> Stashed changes
    }

    protected override void Start()
    {
<<<<<<< Updated upstream
        if (waveDataSO == null || waveDataSO.waves.Count == 0)
        {
            Debug.LogWarning("EnemySpawner: WaveDataSO chưa gán hoặc không có wave nào!");
            return;
=======
        if (wave == null)
        {
            Debug.LogError("Wave is null!");
            yield break;
        }

        if (wave.EnemiesToSpawn == null)
        {
            Debug.LogError("EnemiesToSpawn list is null in wave!");
            yield break;
        }

        if (wave.EnemiesToSpawn.Count == 0)
        {
            Debug.LogError("No enemies to spawn in wave!");
            yield break;
        }

        // 1) Khởi tạo counter = số nhóm trong wave
        _groupsRemaining = wave.EnemiesToSpawn.Count;
        Debug.Log($"Starting wave with {_groupsRemaining} enemy groups");

        // 2) Với mỗi spawnData, start 1 coroutine con
        foreach (var spawnData in wave.EnemiesToSpawn)
        {
            if (spawnData == null)
            {
                Debug.LogError("Found null spawnData in wave.EnemiesToSpawn!");
                _groupsRemaining--;
                continue;
            }
            StartCoroutine(SpawnEnemyGroup(spawnData));
>>>>>>> Stashed changes
        }
        StartCoroutine(RunAllWaves());
    }

<<<<<<< Updated upstream
    private IEnumerator RunAllWaves()
    {
        for (int i = 0; i < waveDataSO.waves.Count; i++)
=======
    private IEnumerator SpawnEnemyGroup(EnemySpawnData spawnData)
    {
        if (spawnData == null)
        {
            Debug.LogError("SpawnData is null!");
            _groupsRemaining--;
            yield break;
        }

        if (string.IsNullOrEmpty(spawnData.EnemyType))
        {
            Debug.LogError("EnemyType is null or empty in SpawnData!");
            _groupsRemaining--;
            yield break;
        }

        Debug.Log($"Spawning enemy group: {spawnData.Count} {spawnData.EnemyType} enemies with interval {spawnData.SpawnInterval}s");
        
        for (int i = 0; i < spawnData.Count; i++)
>>>>>>> Stashed changes
        {
            EnemyWave wave = waveDataSO.waves[i];
            Debug.Log($"--- Bắt đầu Wave {i + 1} ---");
            yield return RunSingleWave(wave, i + 1);
        }
<<<<<<< Updated upstream
        Debug.Log("=== Đã hoàn thành tất cả các wave ===");
        GameSystem.instance.GameWin();
=======

        // Khi group này xong, giảm counter
        _groupsRemaining--;
        Debug.Log($"Group complete. {_groupsRemaining} groups remaining");
>>>>>>> Stashed changes
    }

    private IEnumerator RunSingleWave(EnemyWave wave, int waveNumber)
    {
        // Reset counter
        aliveEnemies = 0;

<<<<<<< Updated upstream
        // Spawn theo từng EnemyData
        foreach (var enemyData in wave.enemies)
        {
            MainCanvas.instance.ShowNextWave(waveNumber, enemyData.count);
            for (int j = 0; j < enemyData.count; j++)
=======
    GameObject GetEnemyPrefab(string name)
    {
        // Map wave types to actual prefab names
        string prefabName = name.ToLower() switch
        {
            "normal" => "Enemy",
            "boss" => "Goblin",
            _ => name
        };

        // Make the comparison case-insensitive
        prefabName = prefabName.ToLower();
        foreach (var enemy in EnemyPrefab)
        {
            if (enemy != null && enemy.name.ToLower().Contains(prefabName))
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        // Đợi đến khi người chơi kill sạch
        yield return new WaitUntil(() => aliveEnemies == 0);

        Debug.Log($"--- Wave {waveNumber} cleared! ---");
        MainCanvas.instance.ShowAllWavesCompleted();
=======
        Debug.LogError($"Enemy prefab not found: {name} (mapped to {prefabName}). Available prefabs: {string.Join(", ", EnemyPrefab.ConvertAll(p => p?.name ?? "null"))}");
        return null;
>>>>>>> Stashed changes
    }

    /// <summary>
    /// Gọi từ Enemy.Die() để giảm counter aliveEnemies
    /// </summary>
    public void NotifyEnemyKilled()
    {
<<<<<<< Updated upstream
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
=======
        if (_spawnAreas == null || _spawnAreas.Count == 0)
        {
            Debug.LogError("No spawn areas set up!");
            return Vector2.zero;
        }
        
        int index = Random.Range(0, _spawnAreas.Count);
        return _spawnAreas[index].GetRandomSpawnPos();
>>>>>>> Stashed changes
    }

    private Vector3 GetRandomSpawnPos()
    {
<<<<<<< Updated upstream
        float y = Random.Range(minY, maxY);
        return new Vector3(spawnX, y, 0f);
    }

    public override Transform Spawn(string prefabName, Vector3 spawnPos, Quaternion rotation)
    {
        return base.Spawn(prefabName, spawnPos, rotation);
=======
        if (transform.childCount == 0) return null;
        return transform.GetChild(Random.Range(0, transform.childCount));
>>>>>>> Stashed changes
    }
}
