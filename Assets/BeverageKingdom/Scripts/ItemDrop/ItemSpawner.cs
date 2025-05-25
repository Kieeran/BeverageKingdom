using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Spawner
{
    public static ItemSpawner Instance;

    private int  spawnRate = 10; // spawn rate in seconds
    protected override void Awake()
    {
        Instance = this;
    }
    public void Spawn(Vector3 position, Quaternion rotation)
    {
        Transform item = RandomPrefabs();
        // them ti le spawn item
        int a =Random.Range(0, 100);
        //GameObject item = Instantiate(itemPrefab, position, rotation, parent);
        if (a < spawnRate)
        {
            base.Spawn(item, position, rotation);
        }
    }
}
