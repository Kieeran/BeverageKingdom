using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class HomeSceneCanvas : MonoBehaviour
{
    public Button StartGameButton;
    public Button ExitGameButton;
    public Button TutorialButton;

    [Header("Tutorial Components")]
    public GameObject TutorialPanel;
    public Button NextButton;
    public Button BackButton;

    public Transform[] TutorialPages;
    private int currentPage = 0;

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

        NextButton.onClick.AddListener(NextPage);
      //  BackButton.onClick.AddListener(PreviousPage);
    }

    void Start()
    {
        SoundManager.Instance?.PlaySoundWithDelay(SoundManager.Instance?.HomeMenuSound, true, 0.8f);
        HideAllPages();
        TutorialPanel.SetActive(false); // Ẩn tutorial lúc đầu
        TutorialButton.onClick.AddListener(() =>
        {
            TutorialPanel.SetActive(true); // Hiện tutorial khi nhấn nút
            currentPage = 0;
            UpdateTutorialPage();
        });
        StartGameButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("PlayScene");
        });
    }

    void UpdateTutorialPage()
    {
        HideAllPages();

        if (currentPage >= 0 && currentPage < TutorialPages.Length)
        {
            TutorialPages[currentPage].gameObject.SetActive(true);
        }

     //   BackButton.interactable = currentPage > 0;
    }

    void HideAllPages()
    {
        foreach (var page in TutorialPages)
        {
            page.gameObject.SetActive(false);
        }
    }

    void NextPage()
    {
        if (currentPage < TutorialPages.Length -1)
        {
            currentPage++;
            UpdateTutorialPage();
        }
        else
        {
            TutorialPanel.SetActive(false);
            // SceneManager.LoadSceneAsync("PlayScene");
            Debug.Log("End");
        }
    }

    void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateTutorialPage();
        }
    }
}
