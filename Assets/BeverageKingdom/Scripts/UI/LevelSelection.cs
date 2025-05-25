using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] RectTransform _container;
    [SerializeField] RectTransform _levelItemUIPrefab;
    [SerializeField] Button _exitButton;

    public int LevelNumber;

    void Awake()
    {
        _exitButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    void Start()
    {
        for (int i = 0; i < LevelNumber; i++)
        {
            GameObject levelItem = Instantiate(_levelItemUIPrefab.gameObject, _container);
            levelItem.SetActive(true);

            LevelItemUI levelItemUI = levelItem.GetComponent<LevelItemUI>();
            levelItemUI.UpdateLevelItemUIText((i + 1).ToString());
        }
    }

    void Update()
    {

    }
}
