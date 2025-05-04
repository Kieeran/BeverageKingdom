using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    public Image HealthBarFillUI;

    [HideInInspector]
    public float HP;

    public float MaxHP;

    void Start()
    {
        MaxHP = HP;
    }

    public void DecreaseHouseHP()
    {
        if (HP == 0) return;

        HP--;
        HealthBarFillUI.fillAmount = HP / MaxHP;
        if (HP == 0)
        {
            Invoke("DelayAndGameOver", 1f);
        }
    }

    void DelayAndGameOver()
    {
        Time.timeScale = 0f;
        GameSystem.instance.GameOver();
    }
}
