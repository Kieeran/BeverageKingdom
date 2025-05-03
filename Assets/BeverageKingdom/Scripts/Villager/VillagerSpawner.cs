using UnityEngine;

public class VillagerSpawner : Spawner
{
    public static VillagerSpawner Instance { get; private set; }
    public string enemy = "Villager";

    [Header("Giới hạn toạ độ Y khi spawn")]
    public float minY = -2f;
    public float maxY = 2f;

    [Header("Cài đặt spawn tự động")]
    public bool autoSpawn = true;
    public float minSpawnInterval = 0.3f;
    public float maxSpawnInterval = 0.5f;
    public float spawnX = 10f; // vị trí X cố định để spawn

    private float nextSpawnTime = 0f;

    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        if (!autoSpawn) return;

        nextSpawnTime -= Time.deltaTime;
        if (nextSpawnTime <= 0f)
        {
            SpawnAtX(spawnX, Quaternion.identity);
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    public Transform SpawnAtX(float xPos, Quaternion rotation)
    {
        float y = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(xPos, y, 0f);
        return Spawn(enemy, spawnPos, rotation);
    }

    public override Transform Spawn(string prefabName, Vector3 spawnPos, Quaternion rotation)
    {
        return base.Spawn(prefabName, spawnPos, rotation);
    }
}
