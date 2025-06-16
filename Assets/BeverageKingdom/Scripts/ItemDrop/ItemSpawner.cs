using UnityEngine;

public class ItemSpawner : Spawner
{
    public static ItemSpawner Instance;
    private int spawnRate = 50; // spawn rate in seconds

    protected override void Awake()
    {
        Instance = this;
        spawnRate = 90;
    }

    public bool RandomChance(float chancePercent)
    {
        return Random.value * 100f < chancePercent;
    }

    public void Spawn(float dropItemChance, Vector3 position, Quaternion rotation)
    {
        if (!RandomChance(dropItemChance)) return;
        
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
