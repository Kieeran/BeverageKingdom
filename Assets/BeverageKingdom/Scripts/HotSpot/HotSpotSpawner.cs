using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HotSpotSpawner : MonoBehaviour
{
    public static HotSpotSpawner Instance;
    [SerializeField] GameObject _hotSpotPrefab;
    [SerializeField] List<Transform> _spawnZones; // 9 vùng
    [SerializeField] float _hotspotSpawnDelay;

    [Header("Warning Effect")]
    public GameObject WarningEffectPrefab;
    [SerializeField] float _warningDuration;
    [SerializeField] float _blinkInterval;

    bool _isSpawning = false;
    float _timer = 0f;
    int index;
    WaveData currentWaveData;

    void Awake()
    {
        Instance = this;

        WarningEffectPrefab.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }

    void Update()
    {
        if (!_isSpawning) return;

        _timer += Time.deltaTime;
        if (_timer >= currentWaveData.HotSpotsToSpawn[index].LocalStartTime)
        {
            SpawnRoutine(currentWaveData.HotSpotsToSpawn[index].Count);
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

    void SpawnRoutine(int count)
    {
        int spawnCount = Mathf.Min(count, _spawnZones.Count);
        List<Transform> selectedZones = new();

        while (selectedZones.Count < spawnCount)
        {
            Transform zone = _spawnZones[Random.Range(0, _spawnZones.Count)];
            if (!selectedZones.Contains(zone))
                selectedZones.Add(zone);
        }

        StartCoroutine(SpawnHotSpots(selectedZones));
    }

    IEnumerator SpawnHotSpots(List<Transform> selectedZones)
    {
        foreach (var zone in selectedZones)
        {
            StartCoroutine(SpawnHotSpot(zone));

            yield return new WaitForSeconds(_hotspotSpawnDelay); // delay giữa các hotspot
        }
    }

    IEnumerator SpawnHotSpot(Transform zone)
    {
        SoundManager.Instance?.PlayAudio(SoundManager.Instance?.WarningHotSpotSound, false);

        GameObject warningEffect = Instantiate(WarningEffectPrefab, zone);
        warningEffect.transform.localPosition = Vector3.zero;
        Image warningEffectImage = warningEffect.transform.GetChild(0).GetComponent<Image>();

        float timer = 0f;
        bool visible = true;
        float interval = Mathf.Max(_blinkInterval, 0.01f);

        while (timer < _warningDuration)
        {
            visible = !visible;
            warningEffectImage.color = new Color(1, 1, 1, visible ? 1f : 0f);
            yield return new WaitForSeconds(interval);
            timer += interval;
        }
        Destroy(warningEffect);

        GameObject hotSpot = Instantiate(_hotSpotPrefab, zone.position, Quaternion.Euler(0, 0, 180));
        hotSpot.SetActive(true);
    }
}