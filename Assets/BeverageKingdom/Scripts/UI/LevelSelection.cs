using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] RectTransform _container;
    [SerializeField] RectTransform _levelItemUIPrefab;
    [SerializeField] Button _exitButton;
    [SerializeField] Button _levelGD1Button;
    [SerializeField] Button _levelGD2Button;
    [SerializeField] Image _levelActiveVisualGD1;
    [SerializeField] Image _levelActiveVisualGD2;

    public int LevelNumber;

    void Awake()
    {
        _exitButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        _levelGD1Button.onClick.AddListener(() =>
        {
            Controller.Instance.ChangeGD(
                1,
                () =>
                {
                    CreateLevelSelection();
                }
            );

            _levelActiveVisualGD1.gameObject.SetActive(true);
            _levelActiveVisualGD2.gameObject.SetActive(false);
        });

        _levelGD2Button.onClick.AddListener(() =>
        {
            Controller.Instance.ChangeGD(
                2,
                () =>
                {
                    CreateLevelSelection();
                }
            );

            _levelActiveVisualGD1.gameObject.SetActive(false);
            _levelActiveVisualGD2.gameObject.SetActive(true);
        });
    }

    void CreateLevelSelection()
    {
        ClearLevelSelection();

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

    void ClearLevelSelection()
    {
        if (_container.childCount == 1) return;

        for (int i = 1; i < _container.childCount; i++)
        {
            Destroy(_container.GetChild(i).gameObject);
        }
    }

    void OnEnable()
    {
        CreateLevelSelection();
    }
}
