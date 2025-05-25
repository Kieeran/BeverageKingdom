using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelItemUI : MonoBehaviour
{
    [SerializeField] RectTransform _lock;
    [SerializeField] TextMeshProUGUI _levelNumber;
    [SerializeField] Button _selectLevelButton;

    public int LevelIndex;

    void Awake()
    {
        _selectLevelButton.onClick.AddListener(() =>
        {
            Controller.Instance.CurrentLevelIndex = LevelIndex;
        });
    }

    public void SetLevelIndex(int index) { LevelIndex = index; }

    public void DisableLevelItemUI()
    {
        _lock.gameObject.SetActive(true);
        _levelNumber.gameObject.SetActive(false);

        _selectLevelButton.targetGraphic = null;
    }

    public void EnableLevelItemUI()
    {
        _lock.gameObject.SetActive(false);
        _levelNumber.gameObject.SetActive(true);

        _selectLevelButton.targetGraphic = GetComponent<Image>();
    }

    public void UpdateLevelItemUIText(string text)
    {
        _levelNumber.text = text;
    }
}
