using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ComboSkill : MonoBehaviour
{
    [Header("Data Combo Skill")]
    [SerializeField] private string comboName;
    [SerializeField] private int comboIntTrigger;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI tmpPro;
    [SerializeField] protected Color color;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float displayDuration = 1.2f;
    [SerializeField] private float punchDuration = 0.5f;
    [SerializeField] private Vector3 punchScale = Vector3.one * 1.5f;
    [SerializeField] private Ease punchEase = Ease.OutBack;

    private Sequence skillSequence;
    protected virtual void Start()
    {
        tmpPro = MainCanvas.instance.tmp; ;
    }

    public virtual void TriggerComboSkill(int currentCombo)
    {
        if (currentCombo % comboIntTrigger == 0)
        {
           ActivateComboSkill();
        }
    }

    public virtual void ActivateComboSkill()
    {
        if (skillSequence != null && skillSequence.IsActive())
            skillSequence.Kill();
        if (tmpPro == null)
            Debug.Log("Text skill not found");
        tmpPro.text = comboName;
        tmpPro.color = color;
        tmpPro.transform.localScale = Vector3.one;

        // Tạo sequence mới
        skillSequence = DOTween.Sequence()
            // Fade in + punch scale đồng thời
            .Append(tmpPro.DOFade(1f, fadeDuration))
            .Join(tmpPro.transform
                .DOPunchScale(punchScale, punchDuration, vibrato: 1, elasticity: 0.5f)
                .SetEase(punchEase))
            // Giữ nguyên trong displayDuration giây
            .AppendInterval(displayDuration)
            // Fade out
            .Append(tmpPro.DOFade(0f, fadeDuration))
            // Hủy reference khi xong
            .OnComplete(() => skillSequence = null);
    }
}
