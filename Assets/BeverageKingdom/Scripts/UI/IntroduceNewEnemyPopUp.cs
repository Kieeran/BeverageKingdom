using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroduceNewEnemyPopUp : MonoBehaviour
{
    public Button ExitButton;
    public List<IntroduceContent> IntroduceContents;

    void Start()
    {
        ExitButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            GameSystem.Instance.ContinueGame();
        });

        int currentLevelIndex = Controller.Instance.CurrentLevelIndex;
        foreach (IntroduceContent content in IntroduceContents)
        {
            if (content.LevelToIntroduce == currentLevelIndex)
            {
                content.gameObject.SetActive(true);
                gameObject.SetActive(true);

                GameSystem.Instance.PauseGame();
                return;
            }
        }
        gameObject.SetActive(false);
    }
}
