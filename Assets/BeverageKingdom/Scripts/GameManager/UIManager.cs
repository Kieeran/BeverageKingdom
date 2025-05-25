using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _playCanvasPrefab;
    [SerializeField] GameObject _homeCanvasPrefab;

    public MainCanvas PlayCanvas { get; private set; }
    public MainCanvas HomeCanvas { get; private set; }

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

        InitGame();
    }

    void InitGame()
    {
        GameObject mainCanvas = Instantiate(_playCanvasPrefab);
        PlayCanvas = mainCanvas.GetComponent<MainCanvas>();
    }
}
