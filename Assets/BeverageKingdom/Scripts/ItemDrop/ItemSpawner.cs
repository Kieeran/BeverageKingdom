using UnityEngine;

public class ItemSpawner : Spawner
{
    public static ItemSpawner Instance;
    private int spawnRate = 50; // spawn rate in seconds

    protected override void Awake()
    {
        Instance = this;
        spawnRate = 30;
    }

    public void Spawn(Vector3 position, Quaternion rotation)
    {
        Transform item = RandomPrefabs();
        // them ti le spawn item
        int a = Random.Range(0, 100);
        Debug.Log("Spawn Rate: " + a + " / " + spawnRate);    
        //GameObject item = Instantiate(itemPrefab, position, rotation, parent);
        if (a < spawnRate)
        {
            base.Spawn(item, position, rotation);
        }
    }
}
