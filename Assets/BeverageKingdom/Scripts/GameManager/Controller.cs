using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public Action<string> OnSceneChange;

    public Transform Player { get; private set; }
    public Transform Env { get; private set; }

    public static Controller Instance { get; private set; }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

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
        SoundManager.Instance?.PlaySoundWithDelay(SoundManager.Instance?.HomeMenuSound, true, 0.3f);
    }

    public void InitInGame()
    {
        Player = Instantiate(_playerPrefab.gameObject).transform;
        Env = Instantiate(_envPrefab.gameObject).transform;
        Instantiate(_spawnVillager.gameObject);
        Instantiate(_comboController.gameObject);
        Instantiate(_projectileSpawner.gameObject);
        Instantiate(_effectSpawner.gameObject);
        Instantiate(_playerInput.gameObject);
        Instantiate(_gameSystem.gameObject);
        Instantiate(_levelController.gameObject);

        Player.GetComponent<JoystickMove>().SetJoystick(UIManager.Instance.PlayCanvas.GetJoystick());
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Chờ đến khi load xong
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Scene " + sceneName + " loaded!");
        OnSceneChange?.Invoke(sceneName);

        if (sceneName == "PlayScene")
        {
            InitInGame();
        }

        else if (sceneName == "HomeScene")
        {
            InitHome();
        }
    }
}