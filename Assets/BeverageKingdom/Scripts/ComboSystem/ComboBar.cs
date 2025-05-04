using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    [SerializeField] private Image slider;
    private int maxCombo; // Giá trị tối đa của thanh combo
    ComboController comboController;

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
        Debug.Log($"ComboBar: "  +maxCombo + "      "+ combo    ); 
        if (combo >= maxCombo)
        {
            //comboController.CurrentCombo = 0;
            comboController.ResetCombo();
            maxCombo += 5;
            EffectSpawner.instance.Spawn(EffectSpawner.LevelUp, Player.instance.transform.position,Quaternion.identity);
            Player.instance.LevelUp();
        }
    }
}
