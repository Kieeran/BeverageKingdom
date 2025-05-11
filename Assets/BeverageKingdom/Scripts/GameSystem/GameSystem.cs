using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public Action OnGameOver;
    public Action OnGameWin;

    public static GameSystem instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize game system components here
        // For example, you can load player data, initialize UI, etc.
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        OnGameOver?.Invoke();
    }

    public void GameWin()
    {
        Time.timeScale = 0f;
        OnGameWin?.Invoke();
    }
}
