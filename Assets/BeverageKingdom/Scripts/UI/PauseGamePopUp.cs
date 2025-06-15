using UnityEngine;
using UnityEngine.UI;

public class PauseGamePopUp : MonoBehaviour
{
    public Button ResumeButton;
    public Button ExitButton;

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

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        GameSystem.Instance.PauseGame();
    }
}