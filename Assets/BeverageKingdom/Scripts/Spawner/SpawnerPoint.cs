using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPoint : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    private EnemySpawner enemySpawner;

    [Header("Cài đặt spawn")]
    public bool autoSpawn = true;

    private float nextSpawnTime = 0f;

    void Start()
    {
        enemySpawner = EnemySpawner.Instance;

        if (spawnPoints.Count == 0)
        {
            foreach (Transform child in transform)
            {
                spawnPoints.Add(child);
            }
        }

        SetNextSpawnTime();
    }

    void Update()
    {
        if (!autoSpawn) return;

        nextSpawnTime -= Time.deltaTime;
        if (nextSpawnTime <= 0f)
        {

            Debug.Log("asdasd");

            Spawn();
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(0.4f, 0.6f);
    }

    public Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("Không có điểm spawn nào trong danh sách!");
            return null;
        }

        int index = Random.Range(0, spawnPoints.Count);
        return spawnPoints[index];
    }

    public void Spawn()
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        if (spawnPoint == null) return;

        enemySpawner.Spawn(EnemySpawner.enemy, spawnPoint.position, spawnPoint.rotation).gameObject.SetActive(true);
    }
}
