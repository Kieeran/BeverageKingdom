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
        InitLevelSelection();
    }

    void InitLevelSelection()
    {
        int currentLevelIndex = Controller.Instance.CurrentLevelIndex;

        for (int i = 0; i < LevelNumber; i++)
        {
            GameObject levelItem = Instantiate(_levelItemUIPrefab.gameObject, _container);
            levelItem.SetActive(true);

            LevelItemUI levelItemUI = levelItem.GetComponent<LevelItemUI>();
            levelItemUI.UpdateLevelItemUIText((i + 1).ToString());
            levelItemUI.SetLevelIndex(i);

            if (i <= currentLevelIndex)
            {
                levelItemUI.EnableLevelItemUI();
                continue;
            }

            levelItemUI.DisableLevelItemUI();
        }
    }

    void OnEnable()
    {
        int currentLevelIndex = Controller.Instance.CurrentLevelIndex;

        for (int i = 1; i < transform.childCount; i++)
        {
            LevelItemUI levelItemUI = _container.transform.GetChild(i).GetComponent<LevelItemUI>();
            if (levelItemUI.IsEnable == true) continue;

            if (i - 1 <= currentLevelIndex)
            {
                levelItemUI.EnableLevelItemUI();
            }
        }
    }
}
