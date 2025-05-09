using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] Transform _playerPrefab;
    [SerializeField] Transform _envPrefab;
    [SerializeField] Transform _spawnEnemy;
    [SerializeField] Transform _spawnVillager;
    [SerializeField] Transform _comboController;
    [SerializeField] Transform _projectileSpawner;
    [SerializeField] Transform _effectSpawner;
    [SerializeField] Transform _playerInput;
    [SerializeField] Transform _gameSystem;

    public Transform Player { get; private set; }
    public Transform Env { get; private set; }

    public static Controller Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);

        InitGame();
    }

    void InitGame()
    {
        SoundManager.Instance?.PlaySoundWithDelay(SoundManager.Instance?.InGameSE, true, 0.3f);

        Player = Instantiate(_playerPrefab.gameObject).transform;
        Env = Instantiate(_envPrefab.gameObject).transform;
        Instantiate(_spawnEnemy.gameObject);
        Instantiate(_spawnVillager.gameObject);
        Instantiate(_comboController.gameObject);
        Instantiate(_projectileSpawner.gameObject);
        Instantiate(_effectSpawner.gameObject);
        Instantiate(_playerInput.gameObject);
        Instantiate(_gameSystem.gameObject);
    }

    void Start()
    {
        Player.GetComponent<JoystickMove>().SetJoystick(UIManager.Instance.MainCanvas.GetJoystick());
    }
}
