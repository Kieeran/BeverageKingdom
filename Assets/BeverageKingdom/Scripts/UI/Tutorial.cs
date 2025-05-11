using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Button NextButton;
    public Button PreviousButton;
    public Button ExitButton;

    public List<RectTransform> TutorialPages;
    private int currentPage = 0;

    void Awake()
    {
        NextButton.onClick.AddListener(NextPage);
        PreviousButton.onClick.AddListener(PreviousPage);
        ExitButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        currentPage = 0;
        SetPage(currentPage);
    }

    void UpdateTutorialPage(int index)
    {
        for (int i = 0; i < TutorialPages.Count; i++)
        {
            TutorialPages[i].gameObject.SetActive(index == i);
        }
    }

    void NextPage()
    {
        currentPage++;
        SetPage(currentPage);
    }

    void SetPage(int index)
    {
        UpdateButtons(index);
        UpdateTutorialPage(index);
    }

    void PreviousPage()
    {
        currentPage--;
        SetPage(currentPage);
    }

    void UpdateButtons(int index)
    {
        NextButton.gameObject.SetActive(index == 0 || index == 1);
        PreviousButton.gameObject.SetActive(index == 1 || index == 2);
    }
}
