using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] Transform _playerPrefab;
    [SerializeField] Transform _envPrefab;
    [SerializeField] Transform _spawnEnemy;
    [SerializeField] Transform __spawnEnemy;
    [SerializeField] Transform _spawnVillager;
    [SerializeField] Transform _comboController;
    [SerializeField] Transform _projectileSpawner;
    [SerializeField] Transform _effectSpawner;
    [SerializeField] Transform _playerInput;
    [SerializeField] Transform _gameSystem;
    [SerializeField] Transform _levelController;

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
        DontDestroyOnLoad(gameObject);

        InitHome();
    }

    void InitHome()
    {
        SoundManager.Instance?.PlaySoundWithDelay(SoundManager.Instance?.InGameSound, true, 0.3f);
    }

    public void InitInGame()
    {
        Player.GetComponent<JoystickMove>().SetJoystick(UIManager.Instance.PlayCanvas.GetJoystick());

        Player = Instantiate(_playerPrefab.gameObject).transform;
        Env = Instantiate(_envPrefab.gameObject).transform;
        Instantiate(_spawnVillager.gameObject);
        Instantiate(_comboController.gameObject);
        Instantiate(_projectileSpawner.gameObject);
        Instantiate(_effectSpawner.gameObject);
        Instantiate(_playerInput.gameObject);
        Instantiate(_gameSystem.gameObject);
        Instantiate(_levelController.gameObject);
    }
}