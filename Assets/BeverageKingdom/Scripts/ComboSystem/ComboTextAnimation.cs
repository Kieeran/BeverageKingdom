using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class ComboTextAnimation : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text comboText;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private float punchDuration = 0.3f;
    [SerializeField] private float punchScale = 1.2f;
    [SerializeField] private Ease punchEase = Ease.OutBack;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (comboText == null) comboText = GetComponentInChildren<TMP_Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;      // ẩn ban đầu
    }

    private void Start()
    {
        ComboController.Instance.OnComboChanged += OnComboChanged;

    }
    private void OnDisable()
    {
        ComboController.Instance.OnComboChanged -= OnComboChanged;
    }

    private void OnComboChanged(int combo)
    {
        if (combo <= 0)
        {
            // ẩn text khi reset combo
            canvasGroup.DOFade(0f, fadeDuration);
        }
        else
        {
            // update nội dung và hiện lên
            comboText.text = combo + "";
            canvasGroup.DOFade(1f, fadeDuration);

            // nhún scale
            transform.localScale = Vector3.one;
            transform
                .DOPunchScale(Vector3.one * (punchScale - 1f), punchDuration, 1, 0.5f)
                .SetEase(punchEase);
        }
    }
}
