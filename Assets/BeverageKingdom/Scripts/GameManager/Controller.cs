using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] Transform _levelController;
    [SerializeField] Transform _itemSpawner;
    [SerializeField] Transform _hotSpotManager;
    [SerializeField] Transform _cheater;

    public ObservableVariable<bool> VisualizeDetectionRange = new(false);
    public ObservableVariable<bool> VisualizeBoundingBox = new(false);

    public Action<string> OnSceneChange;

    public Transform Player { get; private set; }
    public Transform Env { get; private set; }

    [HideInInspector]
    public int CurrentLevelIndex = 0;
    [HideInInspector]
    public int CurrentLevelIndexGD1 = 0;
    [HideInInspector]
    public int CurrentLevelIndexGD2 = 0;
    [HideInInspector]
    public bool IsGD1Active = true;

    public static Controller Instance { get; private set; }

    public bool Cheat;

    void Awake()
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
        Instantiate(_itemSpawner.gameObject);
        Instantiate(_hotSpotManager.gameObject);
        Instantiate(_spawnEnemy.gameObject);

        if (Cheat) Instantiate(_cheater.gameObject);

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

    public void ChangeGD(int index, Action OnChangeGD)
    {
        if (index == 1 && IsGD1Active || index == 2 && !IsGD1Active) return;

        if (index == 1)
        {
            CurrentLevelIndexGD2 = CurrentLevelIndex;

            IsGD1Active = true;
            CurrentLevelIndex = CurrentLevelIndexGD1;
        }

        else if (index == 2)
        {
            CurrentLevelIndexGD1 = CurrentLevelIndex;

            IsGD1Active = false;
            CurrentLevelIndex = CurrentLevelIndexGD2;
        }

        OnChangeGD?.Invoke();
    }

    public Env GetEnv() { return Env.GetComponent<Env>(); }
    public Player GetPlayer() { return Player.GetComponent<Player>(); }
}