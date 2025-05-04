using UnityEngine;

public class House : MonoBehaviour
{
    public float HP;

    public void DecreaseHouseHP()
    {
        if (HP == 0) return;

        HP--;
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
