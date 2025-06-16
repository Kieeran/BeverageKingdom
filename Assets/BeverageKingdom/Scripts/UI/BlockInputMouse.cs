using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockInputMouse : MonoBehaviour
{
    [SerializeField] private Button targetButton;      // Gán button trong Inspector
    public float cooldownTime = 5f;  // Thời gian đếm ngược
    private bool isCoolingDown = false;
    [SerializeField] private Image imageFrame;

    public TMP_Text CoolDownNum;

    void Start()
    {
        targetButton = GetComponent<Button>();
        targetButton.onClick.AddListener(OnButtonClick);
        imageFrame.fillAmount = 0;

        CoolDownNum.text = "";
    }

    void OnButtonClick()
    {
        if (!isCoolingDown)
        {
            StartCoroutine(DisableButtonTemporarily());
        }
    }

    IEnumerator DisableButtonTemporarily()
    {
        isCoolingDown = true;
        targetButton.interactable = false;

        float timeRemaining = cooldownTime;

        CoolDownNum.text = ((int)cooldownTime).ToString();

        int lastRemainingTime = (int)cooldownTime;

        while (timeRemaining > 0)
        {
            imageFrame.fillAmount = timeRemaining / cooldownTime; // Cập nhật hình ảnh khung

            timeRemaining -= Time.deltaTime;

            if ((int)timeRemaining != lastRemainingTime)
            {
                CoolDownNum.text = ((int)timeRemaining).ToString();
                lastRemainingTime = (int)timeRemaining;
            }

            yield return null;
        }

        imageFrame.fillAmount = 0; // Cập nhật hình ảnh khung
        // Reset lại rotation về 0 (nếu cần)

        targetButton.interactable = true;
        isCoolingDown = false;

        CoolDownNum.text = "";
    }
}
