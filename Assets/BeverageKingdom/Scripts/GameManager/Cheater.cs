using UnityEngine;

public class Cheater : MonoBehaviour
{
    public static Cheater Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    bool toggle = false;

    public void ToggleVisualizeHp()
    {
        toggle = !toggle;
        Controller.Instance.GetEnv().UpdateHpInfo(toggle);
        Controller.Instance.GetPlayer().playerHPText.gameObject.SetActive(toggle);
    }
}