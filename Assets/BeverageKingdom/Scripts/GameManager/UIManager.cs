using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] List<Transform> _spawnOnStart;

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
        foreach (Transform obj in _spawnOnStart)
        {
            Instantiate(obj.gameObject);
        }
    }
}
