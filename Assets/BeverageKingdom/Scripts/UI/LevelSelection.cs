using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] RectTransform _container;
    [SerializeField] RectTransform _levelItemUIPrefab;
    [SerializeField] Button _exitButton;
    [SerializeField] Button _levelGD1Button;
    [SerializeField] Button _levelGD2Button;

    [SerializeField] Image _levelGD1Image;
    [SerializeField] Image _levelGD2Image;

    public int LevelNumber = 10; // Set to 10 levels

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

            _levelGD1Image.color = Color.white;
            _levelGD2Image.color = new Color32(0x6A, 0x6A, 0x6A, 0xFF);
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

            _levelGD1Image.color = new Color32(0x6A, 0x6A, 0x6A, 0xFF);
            _levelGD2Image.color = Color.white;
        });

        _levelGD1Image.color = Color.white;
        _levelGD2Image.color = new Color32(0x6A, 0x6A, 0x6A, 0xFF);

        gameObject.SetActive(false);
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

            if (Cheater.Instance != null)
            {
                levelItemUI.EnableLevelItemUI();
            }

            else
            {
                if (i <= currentLevelIndex)
                {
                    levelItemUI.EnableLevelItemUI();
                    continue;
                }

                levelItemUI.DisableLevelItemUI();
            }
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
