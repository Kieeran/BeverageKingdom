using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    }    public void SpawnEnemy(string enemyType)
    {
        Debug.Log($"Attempting to spawn enemy of type: {enemyType}");
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

    int _groupsRemaining;    public IEnumerator SpawnWave(WaveData wave, System.Action CheckSpawnAllEnemies)
    {
        if (wave == null || wave.EnemiesToSpawn == null || wave.EnemiesToSpawn.Count == 0)
        {
            Debug.LogError("Invalid wave data!");
            CheckSpawnAllEnemies?.Invoke();
            yield break;
        }

        // Filter out null spawn data entries
        var validSpawnData = wave.EnemiesToSpawn.Where(x => x != null).ToList();
        if (validSpawnData.Count == 0)
        {
            Debug.LogError("No valid enemy spawn data in wave!");
            CheckSpawnAllEnemies?.Invoke();
            yield break;
        }

        // Initialize counter for enemy groups
        _groupsRemaining = validSpawnData.Count;
        Debug.Log($"Starting wave with {_groupsRemaining} enemy groups");

        // Start spawning each valid group
        foreach (var spawnData in validSpawnData)
        {
            StartCoroutine(SpawnEnemyGroup(spawnData));
            yield return new WaitForSeconds(0.2f); // Increased delay between groups
        }        // Wait until all groups are done spawning
        float waitTime = 0f;
        while (_groupsRemaining > 0)
        {
            waitTime += 0.1f;
            Debug.Log($"Waiting for {_groupsRemaining} groups to finish spawning. Total wait time: {waitTime:F1}s");
            yield return new WaitForSeconds(0.1f);
        }

        int remainingEnemies = transform.childCount;
        Debug.Log($"Wave completed. All enemy groups spawned. {remainingEnemies} enemies currently alive.");
        CheckSpawnAllEnemies?.Invoke();
    }    private IEnumerator SpawnEnemyGroup(EnemySpawnData spawnData)
    {
        if (spawnData == null || string.IsNullOrEmpty(spawnData.EnemyType))
        {
            Debug.LogError($"Invalid spawn data: {(spawnData == null ? "null" : "empty enemy type")}!");
            _groupsRemaining = Mathf.Max(0, _groupsRemaining - 1);
            yield break;
        }

        Debug.Log($"Spawning enemy group: {spawnData.Count} {spawnData.EnemyType} enemies with interval {spawnData.SpawnInterval}s");
        
        for (int i = 0; i < spawnData.Count; i++)
        {
            SpawnEnemy(spawnData.EnemyType);
            yield return new WaitForSeconds(Mathf.Max(0.1f, spawnData.SpawnInterval));
        }

        _groupsRemaining = Mathf.Max(0, _groupsRemaining - 1);
        if (_groupsRemaining > 0)
        {
            Debug.Log($"Group complete. {_groupsRemaining} groups remaining");
        }
        else 
        {
            Debug.Log("All groups completed.");
        }
    }    public bool IsAnyEnemyInContainer()
    {
        int count = 0;
        // Count only active enemies (not being destroyed)
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child != null && child.gameObject.activeInHierarchy)
            {
                count++;
            }
        }
        Debug.Log($"Active enemy count in container: {count} (Total children: {transform.childCount})");
        return count > 0;
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
