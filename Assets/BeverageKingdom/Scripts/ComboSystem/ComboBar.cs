using System;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    [SerializeField] private Image slider;
    private int maxCombo; // Giá trị tối đa của thanh combo
    ComboController comboController;
    public Action OnComboMax;

    private void Start()
    {
        comboController = ComboController.Instance;
        maxCombo = comboController.maxCombo;
        if (slider == null) slider = GetComponentInChildren<Image>();
        //slider.maxValue = ComboController.Instance.maxCombo;
        ComboController.Instance.OnComboChanged += UpdateBar;
        UpdateBar(0);
    }

    private void UpdateBar(int combo)
    {
        slider.fillAmount = (float)combo/maxCombo;
        if (combo >= maxCombo)
        {
            OnComboMax.Invoke();
        }
    }
}
