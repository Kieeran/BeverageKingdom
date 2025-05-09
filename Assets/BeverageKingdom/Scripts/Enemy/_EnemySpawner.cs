using System.Collections.Generic;
using UnityEngine;

public class _EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    public float spawnInterval;

    List<SpawnArea> _spawnAreas = new();

    private float timer;

    void Start()
    {
        Env env = Controller.Instance.Env.GetComponent<Env>();

        _spawnAreas.Add(env.EnemySpawnPosSlot1.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.EnemySpawnPosSlot2.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.EnemySpawnPosSlot3.GetChild(0).GetComponent<SpawnArea>());
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        Instantiate(_enemyPrefab, GetRandomSpawnArea().GetRandomSpawnPos(), Quaternion.identity);
    }

    SpawnArea GetRandomSpawnArea()
    {
        int index = Random.Range(0, 3);
        return _spawnAreas[index];
    }
}
