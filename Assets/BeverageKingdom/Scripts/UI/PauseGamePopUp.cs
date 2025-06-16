using UnityEngine;
using UnityEngine.UI;

public class PauseGamePopUp : MonoBehaviour
{
    public Tutorial Tutorial;

    public Button ResumeButton;
    public Button ExitButton;
    public Button TutorialButton;
    public Button ToggleSound;

    void Awake()
    {
        ResumeButton.onClick.AddListener(() =>
        {
            GameSystem.Instance.ContinueGame();
            gameObject.SetActive(false);
        });

        ExitButton.onClick.AddListener(() =>
        {
            Controller.Instance.ChangeScene("HomeScene");
        });

        TutorialButton.onClick.AddListener(() =>
        {
            Tutorial.gameObject.SetActive(true);
        });

        ToggleSound.onClick.AddListener(() =>
        {
            Debug.Log("Toggle sound");
        });

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        GameSystem.Instance.PauseGame();
    }
}