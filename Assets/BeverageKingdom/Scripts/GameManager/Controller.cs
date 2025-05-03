using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] Transform _playerPrefab;
    [SerializeField] Transform _envPrefab;
    [SerializeField] Transform _spawnEnemy;
    [SerializeField] Transform _comboController;
    [SerializeField] Transform _spawnProjectile;

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
        Instantiate(_envPrefab.gameObject);
        Instantiate(_spawnEnemy.gameObject);
        Instantiate(_comboController.gameObject);
        Instantiate(_spawnProjectile.gameObject);
        Player = Instantiate(_playerPrefab.gameObject).transform;
    }

    void Start()
    {
        Player.GetComponent<JoystickMove>().SetJoystick(UIManager.Instance.MainCanvas.GetJoystick());
    }
}
