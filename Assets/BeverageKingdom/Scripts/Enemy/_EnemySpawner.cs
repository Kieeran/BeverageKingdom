using UnityEngine;

public class _EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    public float spawnInterval;
    public Transform spawnPoint;

    private float timer;

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
        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        Instantiate(_enemyPrefab, pos, Quaternion.identity);
    }
}
