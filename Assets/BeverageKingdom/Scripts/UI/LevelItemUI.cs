using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelItemUI : MonoBehaviour
{
    [SerializeField] RectTransform _hide;
    [SerializeField] TextMeshProUGUI _myText;
    [SerializeField] Button _selectLevelButton;

    public int LevelIndex;
    [HideInInspector]
    public bool IsEnable;

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
        _hide.gameObject.SetActive(true);
        _selectLevelButton.targetGraphic = null;

        IsEnable = false;
    }

    public void EnableLevelItemUI()
    {
        _hide.gameObject.SetActive(false);
        _selectLevelButton.targetGraphic = GetComponent<Image>();

        IsEnable = true;
    }

    public void UpdateLevelItemUIText(string text)
    {
        _myText.text = text;
    }
}
