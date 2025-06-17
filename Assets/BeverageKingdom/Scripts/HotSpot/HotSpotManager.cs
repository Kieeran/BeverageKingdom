using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotManager : MonoBehaviour
{
    [SerializeField] private GameObject hotSpotPrefab;
    [SerializeField] private List<Transform> spawnZones; // 9 vùng
    [SerializeField] private int maxHotSpots = 2;

    private bool isSpawning = false;

    public void TrySpawnHotSpots()
    {
        if (!isSpawning)
            StartCoroutine(SpawnRoutine());
    }

    private void Start()
    {
        TrySpawnHotSpots();
    }

    private IEnumerator SpawnRoutine()
    {
        isSpawning = true;

        int spawnCount = Mathf.Min(maxHotSpots, spawnZones.Count);
        List<Transform> selectedZones = new List<Transform>();

        while (selectedZones.Count < spawnCount)
        {
            Transform zone = spawnZones[Random.Range(0, spawnZones.Count)];
            if (!selectedZones.Contains(zone))
                selectedZones.Add(zone);
        }

        foreach (var zone in selectedZones)
        {
            GameObject hotSpot = Instantiate(hotSpotPrefab, zone.position, Quaternion.Euler(0, 0, 180));
            hotSpot.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f); // delay giữa các hotspot nếu cần
        }

        isSpawning = false;
    }
}