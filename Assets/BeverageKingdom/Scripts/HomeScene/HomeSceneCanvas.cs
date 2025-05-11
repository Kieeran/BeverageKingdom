using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class HomeSceneCanvas : MonoBehaviour
{
    public Button StartGameButton;
    public Button ExitGameButton;
    public Button TutorialButton;

    public RectTransform TutorialPanel;

    void Awake()
    {
        ExitGameButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    void Start()
    {
        SoundManager.Instance?.PlaySoundWithDelay(SoundManager.Instance?.HomeMenuSound, true, 0.8f);

        TutorialPanel.gameObject.SetActive(false);

        TutorialButton.onClick.AddListener(() =>
        {
            TutorialPanel.gameObject.SetActive(true);
        });

        StartGameButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("PlayScene");
        });
    }
}
