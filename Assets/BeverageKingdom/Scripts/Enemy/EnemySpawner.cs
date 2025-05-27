using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public List<GameObject> EnemyPrefab;

    [Header("Enemy Prefab References")]
    public GameObject normalEnemyPrefab;

    private Dictionary<string, EnemySO> _enemyDataCache;

    List<SpawnArea> _spawnAreas = new();

    private void Awake()
    {
        Instance = this;
        
        // Initialize the enemy prefabs list if it's empty
        if (EnemyPrefab == null || EnemyPrefab.Count == 0)
        {
            EnemyPrefab = new List<GameObject>();
            if (normalEnemyPrefab != null)
                EnemyPrefab.Add(normalEnemyPrefab);
        }
    }

    void Start()
    {
        Env env = Controller.Instance.Env.GetComponent<Env>();

        _spawnAreas.Add(env.EnemySpawnPosSlot1.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.EnemySpawnPosSlot2.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.EnemySpawnPosSlot3.GetChild(0).GetComponent<SpawnArea>());

        // Initialize enemy data cache
        LoadEnemyData();

        // Validate enemy prefabs
        if (EnemyPrefab.Count == 0)
        {
            Debug.LogError("No enemy prefabs assigned to EnemySpawner! Please assign normal enemy prefab in the inspector.");
        }
    }

    private void LoadEnemyData()
    {
        _enemyDataCache = new Dictionary<string, EnemySO>();
        
        // Load all enemy scriptable objects
        var scoutData = Resources.Load<EnemySO>("Data/ScoutEnemy");
        var warriorData = Resources.Load<EnemySO>("Data/WarriorEnemy");
        var heavyData = Resources.Load<EnemySO>("Data/HeavyEnemy");

        if (scoutData != null) _enemyDataCache["scout"] = scoutData;
        if (warriorData != null) _enemyDataCache["warrior"] = warriorData;
        if (heavyData != null) _enemyDataCache["heavy"] = heavyData;

        if (_enemyDataCache.Count == 0)
        {
            Debug.LogError("No enemy data found! Make sure ScriptableObjects exist in Resources/Data/");
        }
    }

    public void SpawnEnemy(string enemyType)
    {
        GameObject enemyPrefab = normalEnemyPrefab;
        if (enemyPrefab == null)
        {
            Debug.LogError("Normal enemy prefab not assigned!");
            return;
        }

        // Get the enemy data
        string type = enemyType.ToLower();
        if (!_enemyDataCache.TryGetValue(type, out var enemyData))
        {
            Debug.LogError($"Enemy data not found for type: {enemyType}. Available types: {string.Join(", ", _enemyDataCache.Keys)}");
            return;
        }

        Vector2 spawnPos = GetRandomSpawnPos();
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy.transform.SetParent(transform);

        // Set the enemy data
        var enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.SetEnemyData(enemyData);
        }
        else
        {
            Debug.LogError("Spawned enemy prefab doesn't have Enemy component!");
        }
    }

    int _groupsRemaining;
    public IEnumerator SpawnWave(WaveData wave, System.Action CheckSpawnAllEnemies)
    {
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
        }

        // 3) Chờ cho đến khi tất cả nhóm xong (counter về 0)
        yield return new WaitUntil(() => _groupsRemaining == 0);

        CheckSpawnAllEnemies?.Invoke();
    }

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
        {
            SpawnEnemy(spawnData.EnemyType);
            yield return new WaitForSeconds(spawnData.SpawnInterval);
        }

        // Khi group này xong, giảm counter
        _groupsRemaining--;
        Debug.Log($"Group complete. {_groupsRemaining} groups remaining");
    }

    public bool IsAnyEnemyInContainer()
    {
        return transform.childCount != 0;
    }

    Vector2 GetRandomSpawnPos()
    {
        if (_spawnAreas == null || _spawnAreas.Count == 0)
        {
            Debug.LogError("No spawn areas set up!");
            return Vector2.zero;
        }
        
        int index = Random.Range(0, _spawnAreas.Count);
        return _spawnAreas[index].GetRandomSpawnPos();
    }

    public Transform GetRandomEnemy()
    {
        if (transform.childCount == 0) return null;
        return transform.GetChild(Random.Range(0, transform.childCount));
    }
}
