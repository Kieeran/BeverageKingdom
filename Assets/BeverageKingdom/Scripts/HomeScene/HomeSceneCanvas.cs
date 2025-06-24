using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class HomeSceneCanvas : MonoBehaviour
{
    public Button StartGameButton;
    public Button ExitGameButton;
    public Button SettingsButton;
    public Transform LevelSelection;
    public Transform SettingsPopUp;

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
        SettingsButton.onClick.AddListener(() =>
        {
            SettingsPopUp.gameObject.SetActive(true);
        });
        StartGameButton.onClick.AddListener(() =>
        {
            LevelSelection.gameObject.SetActive(true);
        });

        LevelSelection.gameObject.SetActive(false);
    }
}
