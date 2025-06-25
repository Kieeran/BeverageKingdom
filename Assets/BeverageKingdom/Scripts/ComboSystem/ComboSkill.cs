using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ComboSkill : MonoBehaviour
{
    [Header("Data Combo Skill")]
    [SerializeField] private string comboName;
    [SerializeField] private int comboIntTrigger;

    [Header("UI")]
    [SerializeField] protected Color color;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float displayDuration = 1.2f;
    [SerializeField] private float punchDuration = 0.5f;
    [SerializeField] private Vector3 punchScale = Vector3.one * 1.2f;
    [SerializeField] private Ease punchEase = Ease.OutBack;

    private Sequence skillSequence;
    protected virtual void Start()
    {
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

        Image currentSkillVisualize;
        if (comboName == "Electric")
        {
            currentSkillVisualize = PlayCanvas.Instance.SkillVisualize.ThunderSkillVisualize;
        }

        else if (comboName == "Ice Freeze")
        {
            currentSkillVisualize = PlayCanvas.Instance.SkillVisualize.IceSkillVisualize;
        }
        else
        {
            Debug.LogWarning($"Skill '{comboName}' not found or not valid");
            return;
        }

        // Bật image trước khi hiệu ứng
        currentSkillVisualize.gameObject.SetActive(true);

        // Reset trạng thái image
        Color originalColor = currentSkillVisualize.color;
        currentSkillVisualize.color = new Color(
            originalColor.r, originalColor.g, originalColor.b, 0f);

        currentSkillVisualize.transform.localScale = Vector3.one;

        // Tạo hiệu ứng DOTween
        skillSequence = DOTween.Sequence()
            .Append(currentSkillVisualize.DOFade(1f, fadeDuration))
            .Join(currentSkillVisualize.transform
                .DOPunchScale(punchScale, punchDuration, vibrato: 1, elasticity: 0.5f)
                .SetEase(punchEase))
            .AppendInterval(displayDuration)
            .Append(currentSkillVisualize.DOFade(0f, fadeDuration))
            .OnComplete(() =>
            {
                currentSkillVisualize.gameObject.SetActive(false); // Tắt sau khi hoàn thành
                skillSequence = null;
            });
    }
}