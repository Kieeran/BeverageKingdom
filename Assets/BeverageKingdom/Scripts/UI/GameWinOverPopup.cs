using UnityEngine;
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
            Controller.Instance.ChangeScene("PlayScene");
        });

        ExitButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SoundManager.Instance?.StopSound();
            Controller.Instance.ChangeScene("HomeScene");
        });

        ContinueButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            Controller.Instance.CurrentLevelIndex++;
            Controller.Instance.ChangeScene("PlayScene");
        });

        gameObject.SetActive(false);
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
