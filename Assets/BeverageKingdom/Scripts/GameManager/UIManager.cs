using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _mainCanvasPrefab;

    public MainCanvas MainCanvas { get; private set; }

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
        GameObject mainCanvas = Instantiate(_mainCanvasPrefab);
        MainCanvas = mainCanvas.GetComponent<MainCanvas>();
    }
}
