using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class HomeSceneCanvas : MonoBehaviour
{
    public Button StartGameButton;
    public Button ExitGameButton;
    public Button TutorialButton;

    public Button LevelSelectionButton;
    public LevelSelection LevelSelection;

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
        TutorialPanel.gameObject.SetActive(false);
        LevelSelection.gameObject.SetActive(false);

        TutorialButton.onClick.AddListener(() =>
        {
            TutorialPanel.gameObject.SetActive(true);
        });

        StartGameButton.onClick.AddListener(() =>
        {
            Controller.Instance.ChangeScene("PlayScene");
        });

        LevelSelectionButton.onClick.AddListener(() =>
        {
            LevelSelection.gameObject.SetActive(true);
        });
    }
}
