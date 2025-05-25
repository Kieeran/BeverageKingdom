using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _playCanvasPrefab;
    [SerializeField] GameObject _homeCanvasPrefab;

    public HomeSceneCanvas HomeCanvas { get; private set; }
    public MainCanvas PlayCanvas { get; private set; }

    public static UIManager Instance { get; private set; }

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

    void Start()
    {
        Controller.Instance.OnSceneChange += OnSceneChange;
    }

    void OnSceneChange(string sceneName)
    {
        if (sceneName == "PlayScene")
        {
            InitInGame();
        }

        else if (sceneName == "HomeScene")
        {
            InitHome();
        }
    }

    public void InitHome()
    {
        GameObject homeCanvas = Instantiate(_homeCanvasPrefab);
        HomeCanvas = homeCanvas.GetComponent<HomeSceneCanvas>();
    }

    public void InitInGame()
    {
        GameObject mainCanvas = Instantiate(_playCanvasPrefab);
        PlayCanvas = mainCanvas.GetComponent<MainCanvas>();
    }
}
