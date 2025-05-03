using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSkill : MonoBehaviour
{
    [Header("data combo skill")]
    [SerializeField] private string comboName;
    [SerializeField] private int comboIntTrigger;
    
    public virtual void TriggerComboSkill(int currentCombo)
    {
        if (currentCombo % comboIntTrigger ==0)
        {
            ActivateComboSkill();
        }
    }   
    protected virtual void ActivateComboSkill()
    {

        Debug.Log(comboName);

    }
}
