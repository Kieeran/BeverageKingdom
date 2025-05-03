using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] Transform _playerPrefab;
    [SerializeField] Transform _envPrefab;
    [SerializeField] Transform _spawnEnemy;
    [SerializeField] Transform _spawnVillager;

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

        InitGame();
    }

    void InitGame()
    {
        Player = Instantiate(_playerPrefab.gameObject).transform;
        Env = Instantiate(_envPrefab.gameObject).transform;
        Instantiate(_spawnEnemy.gameObject);
        Instantiate(_spawnVillager.gameObject);
    }

    void Start()
    {
        Player.GetComponent<JoystickMove>().SetJoystick(UIManager.Instance.MainCanvas.GetJoystick());
    }
}
