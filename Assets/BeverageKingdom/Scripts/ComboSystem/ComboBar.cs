using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    [SerializeField] private Image slider;
    private int maxCombo = 100; // Giá trị tối đa của thanh combo
    private void Start()
    {   
        maxCombo = ComboController.Instance.maxCombo;
        if (slider == null) slider = GetComponentInChildren<Image>();
        //slider.maxValue = ComboController.Instance.maxCombo;
        ComboController.Instance.OnComboChanged += UpdateBar;
        UpdateBar(0);
    }

    private void UpdateBar(int combo)
    {
        slider.fillAmount = combo/maxCombo;
    }
}
