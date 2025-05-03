using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] Transform _playerPrefab;
    [SerializeField] Transform _envPrefab;
    [SerializeField] Transform _spawnEnemy;

    public Transform Player { get; private set; }

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
        Instantiate(_envPrefab.gameObject);
        Instantiate(_spawnEnemy.gameObject);
    }

    void Start()
    {
        Player.GetComponent<JoystickMove>().SetJoystick(UIManager.Instance.MainCanvas.GetJoystick());
    }
}
