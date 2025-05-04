using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class ComboTextAnimation : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private TMP_Text suffixText;         // Text for threshold labels

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private float punchDuration = 0.3f;
    [SerializeField] private float punchScale = 1.2f;
    [SerializeField] private Ease punchEase = Ease.OutBack;

    [Header("Suffix Burst Settings")]
    [SerializeField] private float suffixBurstDuration = 0.4f;
    [SerializeField] private float suffixBurstScale = 1.5f;
    [SerializeField] private Ease suffixBurstEase = Ease.OutBack;

    [Header("Color Settings")]
    [SerializeField] private Gradient comboColorGradient;  // Assign a gradient in Inspector
    [SerializeField] private int maxComboForGradient = 50;

    [Header("Thresholds & Labels")]
    [SerializeField] private int excellentThreshold = 20;
    [SerializeField] private string excellentLabel = "EXCELLENT!";

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (comboText == null) comboText = GetComponentInChildren<TMP_Text>();
        if (suffixText == null)
            suffixText = transform.Find("SuffixText")?.GetComponent<TMP_Text>();

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;      // hidden initially

        if (suffixText != null)
        {
            suffixText.text = string.Empty;
        }
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
            // fade out on reset
            canvasGroup.DOFade(0f, fadeDuration);
        }
        else
        {
            // update combo text
            comboText.text = combo.ToString();

            // gradient color for combo
            float t = Mathf.Clamp01((float)combo / maxComboForGradient);
            comboText.color = comboColorGradient.Evaluate(t);

            // fade in combo
            canvasGroup.DOFade(1f, fadeDuration);

            // punch scale for combo
            transform.localScale = Vector3.one;
            transform
                .DOPunchScale(Vector3.one * (punchScale - 1f), punchDuration, 1, 0.5f)
                .SetEase(punchEase);

            // handle suffix burst when threshold reached
            if (suffixText != null)
            {
                bool isExcellent = combo >= excellentThreshold;
                suffixText.text = isExcellent ? excellentLabel : string.Empty;

                if (isExcellent)
                {
                    // fade and burst scale animation
                  //  suffixText.canvasRenderer.SetAlpha(0f);
                    suffixText.DOFade(1f, fadeDuration);

                    suffixText.transform.localScale = Vector3.zero;
                    suffixText.transform
                        .DOScale(Vector3.one * suffixBurstScale, suffixBurstDuration)
                        .SetEase(suffixBurstEase)
                        .OnComplete(() =>
                        {
                            // return to normal size after burst

                            Debug.Log("bbbbbbbbbbbbb");

                            suffixText.transform.DOScale(Vector3.one, punchDuration).SetEase(Ease.InBack);
                        });
                }
            }
        }
    }
}
