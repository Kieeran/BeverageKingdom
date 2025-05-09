using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;

    public GameObject EnemyPrefab;

    public LevelData levelData;
    private int currentWaveIndex = 0;
    private float timer = 0f;
    private bool isSpawningWave = false;

    List<SpawnArea> _spawnAreas = new();

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
    }

    void Update()
    {
        if (currentWaveIndex >= levelData.Waves.Count) return;

        timer += Time.deltaTime;

        if (timer <= _levelDuration)
        {
            UIManager.Instance.MainCanvas.UpdateLevelProgressBar(timer / _levelDuration);
        }

        WaveData wave = levelData.Waves[currentWaveIndex];

        if (!isSpawningWave && timer >= wave.StartTime)
        {
            StartCoroutine(SpawnWave(wave));
            isSpawningWave = true;
        }
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