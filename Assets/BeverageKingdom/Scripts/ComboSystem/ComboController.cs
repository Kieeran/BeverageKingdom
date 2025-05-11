using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    public static ComboController Instance { get; private set; }
    [SerializeField] private List<ComboSkill> skillList = new List<ComboSkill>();

    [Header("Cài đặt combo")]
    public float comboResetDelay = 2f;   // Time before decay starts
    public float decayRate = 1f;         // How many combo points to lose per second
    public int maxCombo = 30;

    public int CurrentCombo { get; private set; }

    private float resetTimer;
    private bool isDecaying = false;
    // private float decayTimer = 0f;  // Track time for decay
    private float lastDecayTime = 0f; // Track when we last decayed

    public event Action<int> OnComboChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        maxCombo = 30;
        resetTimer = comboResetDelay;
    }

    private void Update()
    {
        // If we have a combo, count down to start decay
        if (CurrentCombo > 0)
        {
            if (!isDecaying)
            {
                resetTimer -= Time.deltaTime;
                if (resetTimer <= 0f)
                {
                    isDecaying = true;
                    // decayTimer = 0f;
                    lastDecayTime = Time.time;
                    Debug.Log("Starting combo decay");
                }
            }
            else
            {
                // If we're in decay mode, gradually reduce combo
                float timeSinceLastDecay = Time.time - lastDecayTime;
                if (timeSinceLastDecay >= 1f / decayRate) // Time needed to lose 1 point
                {
                    int decayPoints = Mathf.FloorToInt(timeSinceLastDecay * decayRate);
                    int newCombo = Mathf.Max(0, CurrentCombo - decayPoints);

                    if (newCombo != CurrentCombo)
                    {
                        CurrentCombo = newCombo;
                        OnComboChanged?.Invoke(CurrentCombo);
                        Debug.Log($"Combo decaying: {CurrentCombo} (Rate: {decayRate} points/sec)");
                    }

                    lastDecayTime = Time.time;

                    if (CurrentCombo == 0)
                    {
                        isDecaying = false;
                        Debug.Log("Combo decay complete");
                    }
                }
            }
        }
    }

    // Called when you hit/kill an enemy
    public void AddCombo(int amount = 1)
    {
        CurrentCombo = Mathf.Min(CurrentCombo + amount, maxCombo);
        resetTimer = comboResetDelay;
        isDecaying = false;
        // decayTimer = 0f;
        lastDecayTime = Time.time;
        OnComboChanged?.Invoke(CurrentCombo);
        Debug.Log($"Combo increased to {CurrentCombo}");
        foreach (ComboSkill comboSkill in skillList)
        {
            comboSkill.TriggerComboSkill(CurrentCombo);
        }
    }

    public void ResetCombo()
    {
        CurrentCombo = 0;
        isDecaying = false;
        resetTimer = comboResetDelay;
        // decayTimer = 0f;
        lastDecayTime = Time.time;
        OnComboChanged?.Invoke(CurrentCombo);
        Debug.Log("Combo reset to 0");
    }
}
