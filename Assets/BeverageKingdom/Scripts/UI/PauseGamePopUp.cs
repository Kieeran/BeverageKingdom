using UnityEngine;
using UnityEngine.UI;

public class PauseGamePopUp : MonoBehaviour
{
    public Tutorial Tutorial;

    public Button ResumeButton;
    public Button ExitButton;
    public Button TutorialButton;
    public Button SoundOn;
    public Button SoundOff;
    public Button MusicOn;
    public Button MusicOff;

    void Awake()
    {
        ResumeButton.onClick.AddListener(() =>
        {
            GameSystem.Instance.ContinueGame();
            gameObject.SetActive(false);
        });

        ExitButton.onClick.AddListener(() =>
        {
            GameSystem.Instance.ContinueGame();
            Controller.Instance.ChangeScene("HomeScene");
        });

        TutorialButton.onClick.AddListener(() =>
        {
            Tutorial.gameObject.SetActive(true);
        });

        SoundOn.onClick.AddListener(() =>
        {
            SoundOn.gameObject.SetActive(false);
            SoundOff.gameObject.SetActive(true);

            SoundManager.Instance.ToggleSound(false);
            Debug.Log("Turn off sound");
        });

        SoundOff.onClick.AddListener(() =>
        {
            SoundOn.gameObject.SetActive(true);
            SoundOff.gameObject.SetActive(false);

            SoundManager.Instance.ToggleSound(true);
            Debug.Log("Turn on sound");
        });

        MusicOn.onClick.AddListener(() =>
        {
            MusicOn.gameObject.SetActive(false);
            MusicOff.gameObject.SetActive(true);

            SoundManager.Instance.ToggleMusic(false);
            Debug.Log("Turn off music");
        });

        MusicOff.onClick.AddListener(() =>
        {
            MusicOn.gameObject.SetActive(true);
            MusicOff.gameObject.SetActive(false);

            SoundManager.Instance.ToggleMusic(true);
            Debug.Log("Turn on music");
        });

        SoundOn.gameObject.SetActive(SoundManager.Instance.SoundToggle);
        SoundOff.gameObject.SetActive(!SoundManager.Instance.SoundToggle);

        MusicOn.gameObject.SetActive(SoundManager.Instance.MusicToggle);
        MusicOff.gameObject.SetActive(!SoundManager.Instance.MusicToggle);

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        GameSystem.Instance.PauseGame();
    }
}