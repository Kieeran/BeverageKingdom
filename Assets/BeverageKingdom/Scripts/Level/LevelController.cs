using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    MileStone _mileStoneProgressBar;

    public LevelData levelData;
    private int currentWaveIndex = 0;
    private float timer = 0f;

    bool _isLevelComplete;
    bool _isSpawnAllEnemies;
    float _levelDuration;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _levelDuration = levelData.Waves[^1].StartTime;

        _isLevelComplete = false;
        _isSpawnAllEnemies = false;

        _mileStoneProgressBar = UIManager.Instance.PlayCanvas.MileStoneProgressBar;

        InitMarker();
    }

    void InitMarker()
    {
        for (int i = 1; i < levelData.Waves.Count; i++)
        {
            RectTransform markerRect = _mileStoneProgressBar.PlaceTimeMarker(levelData.Waves[i].StartTime, _levelDuration);

            if (i == levelData.Waves.Count - 1)
            {
                _mileStoneProgressBar.UpsizeMarker(markerRect, 50f);
            }
        }
    }

    void Update()
    {
        if (!EnemySpawner.Instance.IsAnyEnemyInContainer() && _isLevelComplete == false && _isSpawnAllEnemies == true)
        {
            _isLevelComplete = true;
            GameSystem.instance.GameWin();
        }

        if (currentWaveIndex >= levelData.Waves.Count)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer <= _levelDuration)
        {
            UIManager.Instance.PlayCanvas.UpdateLevelProgressBar(timer / _levelDuration);
        }

        WaveData wave = levelData.Waves[currentWaveIndex];

        if (timer >= wave.StartTime)
        {
            _mileStoneProgressBar.UpdateCompleteMileStone(currentWaveIndex);
            StartCoroutine(EnemySpawner.Instance.SpawnWave(
                wave,
                () =>
                {
                    if (currentWaveIndex == levelData.Waves.Count - 1)
                    {
                        _isSpawnAllEnemies = true;
                        Debug.Log("completed.");
                    }
                }
            ));
            currentWaveIndex++;
        }
    }
}