using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinOverPopup : MonoBehaviour
{
    public Image GameWinImage;
    public Image GameOverImage;

    public Button ExitButton;
    public Button PlayAgainButton;
    public Button ContinueButton;

    void Awake()
    {
        PlayAgainButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync("PlayScene");
        });

        ExitButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;

            SoundManager.Instance?.StopSound();
            SceneManager.LoadSceneAsync("HomeScene");
        });
    }

    public void OnGameOver()
    {
        GameOverImage.gameObject.SetActive(true);
        GameWinImage.gameObject.SetActive(false);
        ContinueButton.gameObject.SetActive(false);
    }

    public void OnGameWin()
    {
        GameOverImage.gameObject.SetActive(false);
        GameWinImage.gameObject.SetActive(true);
        ContinueButton.gameObject.SetActive(true);
    }
}
