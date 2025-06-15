using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    public Image HealthBarFillUI;
    public TMP_Text HPText;

    [HideInInspector]
    public float HP;

    public float MaxHP;

    void Start()
    {
        MaxHP = HP;

        HPText.text = $"{HP}/{MaxHP}";
        HPText.gameObject.SetActive(false);
    }

    public void ApplyDamageHouse(int damage)
    {
        if (HP == 0) return;

        HP -= damage;
        HealthBarFillUI.fillAmount = HP / MaxHP;
        if (HP == 0)
        {
            Invoke("DelayAndGameOver", 1f);
        }

        HPText.text = $"{HP}/{MaxHP}";
    }

    void DelayAndGameOver()
    {
        GameSystem.Instance.GameOver();
    }
}
