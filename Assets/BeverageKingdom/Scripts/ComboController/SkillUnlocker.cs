using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SkillData
{
    public string skillName;
    public int comboThreshold;    
    public Button skillButton;     
}

public class SkillUnlocker : MonoBehaviour
{
    [SerializeField] private List<SkillData> skills;

    private void Start()
    {
        // Ban đầu disable tất cả button
        foreach (var s in skills)
            s.skillButton.interactable = false;

        ComboController.Instance.OnComboChanged += CheckSkills;
    }

    private void CheckSkills(int combo)
    {
        foreach (var s in skills)
        {
            if (!s.skillButton.interactable && combo >= s.comboThreshold)
                s.skillButton.interactable = true;
        }
    }
}
