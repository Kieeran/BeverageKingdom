using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotSpawner : MonoBehaviour
{
    public static HotSpotSpawner Instance;
    [SerializeField] GameObject _hotSpotPrefab;
    [SerializeField] List<Transform> _spawnZones; // 9 vùng
    [SerializeField] float _hotspotSpawnDelay;

    bool _isSpawning = false;
    float _timer = 0f;
    int index;
    WaveData currentWaveData;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!_isSpawning) return;

        _timer += Time.deltaTime;
        if (_timer >= currentWaveData.HotSpotsToSpawn[index].LocalStartTime)
        {
            StartCoroutine(SpawnRoutine(currentWaveData.HotSpotsToSpawn[index].Count));
            index++;

            if (index == currentWaveData.HotSpotsToSpawn.Count)
            {
                _isSpawning = false;
            }
        }
    }

    public void SpawnWave(WaveData waveData)
    {
        currentWaveData = waveData;

        if (currentWaveData.HotSpotsToSpawn.Count <= 0) return;

        _timer = 0;
        _isSpawning = true;
        index = 0;
    }

    IEnumerator SpawnRoutine(int count)
    {
        int spawnCount = Mathf.Min(count, _spawnZones.Count);
        List<Transform> selectedZones = new();

        while (selectedZones.Count < spawnCount)
        {
            Transform zone = _spawnZones[Random.Range(0, _spawnZones.Count)];
            if (!selectedZones.Contains(zone))
                selectedZones.Add(zone);
        }

        foreach (var zone in selectedZones)
        {
            GameObject hotSpot = Instantiate(_hotSpotPrefab, zone.position, Quaternion.Euler(0, 0, 180));
            hotSpot.SetActive(true);
            yield return new WaitForSeconds(_hotspotSpawnDelay); // delay giữa các hotspot
        }
    }
}