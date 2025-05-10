using System;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image fillImage; // Đây là fill image của thanh tiến độ
    [SerializeField] private ThresholdMarker lowThresholdMarker;
    [SerializeField] private ThresholdMarker highThresholdMarker;
    [SerializeField] private ThresholdMarker maxThresholdMarker;
    
    [Header("Threshold Settings")]
    [SerializeField] private int lowThreshold = 10;  // Khi skill sét đánh nhiều quái hơn
    [SerializeField] private int highThreshold = 20; // Khi skill Ice Freeze kích hoạt
    [SerializeField] private int maxThreshold = 30; // Khi skill sét đánh tất cả quái
    // Public properties to access thresholds
    public int LowThreshold => lowThreshold;
    public int HighThreshold => highThreshold;
    public int MaxThreshold => maxThreshold;
    
    private int maxCombo;
    private ComboController comboController;
    public Action OnComboMax;

    private void Awake()
    {
        // Try to find the fill image if not assigned
        if (fillImage == null)
        {
            fillImage = GetComponentInChildren<Image>();
            if (fillImage == null)
            {
                Debug.LogError("ComboBar: No Image component found! Please assign a fill image in the inspector.");
            }
        }
    }

    private void Start()
    {
        comboController = ComboController.Instance;
        maxCombo = comboController.maxCombo;
        
        // Đặt vị trí các hình ảnh theo ngưỡng
        if (lowThresholdMarker != null)
        {
            RectTransform lowRect = lowThresholdMarker.GetComponent<RectTransform>();
            lowRect.anchorMin = new Vector2((float)lowThreshold / maxCombo, 0);
            lowRect.anchorMax = new Vector2((float)lowThreshold / maxCombo, 1);
        }
        
        if (highThresholdMarker != null)
        {
            RectTransform highRect = highThresholdMarker.GetComponent<RectTransform>();
            highRect.anchorMin = new Vector2((float)highThreshold / maxCombo, 0);
            highRect.anchorMax = new Vector2((float)highThreshold / maxCombo, 1);
        }

        if (maxThresholdMarker != null)
        {
            RectTransform maxRect = maxThresholdMarker.GetComponent<RectTransform>();
            maxRect.anchorMin = new Vector2((float)highThreshold / maxCombo, 0);
            maxRect.anchorMax = new Vector2((float)highThreshold / maxCombo, 1);
        }
        
        ComboController.Instance.OnComboChanged += UpdateBar;
        UpdateBar(0);
    }

    private void UpdateBar(int combo)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = (float)combo/maxCombo;
        }
        
        // Cập nhật trạng thái của các hình ảnh theo combo
        if (lowThresholdMarker != null)
        {
            lowThresholdMarker.SetActive(combo >= lowThreshold);
        }
        
        if (highThresholdMarker != null)
        {
            highThresholdMarker.SetActive(combo >= highThreshold);
        }
        
        if (maxThresholdMarker != null)
        {
            maxThresholdMarker.SetActive(combo >= maxCombo);
        }

        if (combo >= maxCombo)
        {
            OnComboMax.Invoke();
        }
    }
}
