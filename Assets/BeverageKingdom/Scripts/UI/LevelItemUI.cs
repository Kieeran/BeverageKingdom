using TMPro;
using UnityEngine;

public class LevelItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _myText;

    public void UpdateLevelItemUIText(string text)
    {
        _myText.text = text;
    }
}
