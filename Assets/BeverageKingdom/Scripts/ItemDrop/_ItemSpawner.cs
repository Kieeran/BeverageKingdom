using System.Collections.Generic;
using UnityEngine;

public class _ItemSpawner : MonoBehaviour
{
    public List<Transform> ItemPrefabs;

    public static _ItemSpawner Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnRandomItemAtPos(Vector2 pos)
    {
        int randomIndex = Random.Range(0, ItemPrefabs.Count);

        GameObject randomItem = Instantiate(ItemPrefabs[randomIndex].gameObject);
        randomItem.transform.position = pos;

        Debug.Log("Spawn item " + randomIndex);
    }
}
