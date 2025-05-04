using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    public static ComboController Instance { get; private set; }
    [SerializeField] private List<ComboSkill> skillList = new List<ComboSkill>();    
    

    [Header("Cài đặt combo")]
    public float comboResetDelay = 2f;   
    public int maxCombo = 25;            

    public int CurrentCombo { get; private set; }

    private float resetTimer;

    public event Action<int> OnComboChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        maxCombo = 25;
    }
    private void Update()
    {
        // Nếu đang có combo, đếm ngược để reset
        if (CurrentCombo > 0)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0f)
                ResetCombo();
        }
    }

    // Gọi khi bạn đánh trúng/quái chết
    public void AddCombo(int amount = 1)
    {
        CurrentCombo = Mathf.Min(CurrentCombo + amount, maxCombo);
        resetTimer = comboResetDelay;
        OnComboChanged?.Invoke(CurrentCombo);
        foreach (ComboSkill comboSkill in skillList)
        {
            comboSkill.TriggerComboSkill(CurrentCombo);
        }
    }

    public void ResetCombo()
    {
        CurrentCombo = 0;
        OnComboChanged?.Invoke(CurrentCombo);
    }
}
