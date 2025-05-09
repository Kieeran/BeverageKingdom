using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlockInputMouse : MonoBehaviour
{
    [SerializeField] private Button targetButton;      // Gán button trong Inspector
    public float cooldownTime = 5f;  // Thời gian đếm ngược
    private bool isCoolingDown = false;
    [SerializeField] private Image imageFrame;

    void Start()
    {   
        targetButton = GetComponent<Button>();
        targetButton.onClick.AddListener(OnButtonClick);
        imageFrame.fillAmount = 0;
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

        while (timeRemaining > 0)
        {
            imageFrame.fillAmount = timeRemaining / cooldownTime; // Cập nhật hình ảnh khung

            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        imageFrame.fillAmount = 0; // Cập nhật hình ảnh khung
        // Reset lại rotation về 0 (nếu cần)

        targetButton.interactable = true;
        isCoolingDown = false;
    }
}
