using UnityEngine;
using UnityEngine.UI;

public class ThresholdMarker : MonoBehaviour
{
    [SerializeField] private Image markerImage;
    [SerializeField] private Color activeColor = Color.yellow;
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private float width = 2f;

    private void Awake()
    {
        if (markerImage == null)
            markerImage = GetComponent<Image>();
            
        // Set the width of the marker
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
    }

    public void SetActive(bool active)
    {
        if (markerImage != null)
            markerImage.color = active ? activeColor : inactiveColor;
    }
} 