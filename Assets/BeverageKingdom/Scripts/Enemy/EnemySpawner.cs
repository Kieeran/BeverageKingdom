using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public List<GameObject> EnemyPrefab;

    List<SpawnArea> _spawnAreas = new();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Env env = Controller.Instance.Env.GetComponent<Env>();

        _spawnAreas.Add(env.EnemySpawnPosSlot1.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.EnemySpawnPosSlot2.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.EnemySpawnPosSlot3.GetChild(0).GetComponent<SpawnArea>());
    }

    public void SpawnEnemy(string enemyName)
    {
        GameObject enemyPrefab = GetEnemyPrefab(enemyName);
        if (enemyPrefab == null)
        {
            Debug.LogError($"Enemy prefab not found: {enemyName}");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, GetRandomSpawnPos(), Quaternion.identity);
        enemy.transform.SetParent(transform);
    }

    int _groupsRemaining;
    public IEnumerator SpawnWave(WaveData wave, System.Action CheckSpawnAllEnemies)
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

        CheckSpawnAllEnemies?.Invoke();
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

    public bool IsAnyEnemyInContainer()
    {
        return transform.childCount != 0;
    }

    GameObject GetEnemyPrefab(string name)
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

    Vector2 GetRandomSpawnPos()
    {
        int index = Random.Range(0, 3);
        return _spawnAreas[index].GetRandomSpawnPos();
    }

    public Transform GetRandomEnemy()
    {
        int random = Random.Range(0, transform.childCount);
        Transform enemy = transform.GetChild(random);
        return enemy;
    }
}
