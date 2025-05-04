using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeSceneCanvas : MonoBehaviour
{
    public Button StartGameButton;
    public Button ExitGameButton;

    void Awake()
    {
        StartGameButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("PlayScene");
        });

        ExitGameButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }
}
