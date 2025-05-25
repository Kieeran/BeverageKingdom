using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MileStone : MonoBehaviour
{
    RectTransform _rectTransform;
    public GameObject MarkerPrefab;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void UpsizeMarker(RectTransform marker, float extrasize)
    {
        marker.sizeDelta = new Vector2(marker.sizeDelta.x + extrasize, marker.sizeDelta.y + extrasize);
    }

    public void UpdateCompleteMileStone(int index)
    {
        Image mileStone = transform.GetChild(index).GetComponent<Image>();
        mileStone.color = Color.white;
    }

    public void ClearTimeMarker()
    {
        // Vì chứa cả prefab marker đã disabled nên nếu có ít hơn 2 obj tức là hiện đang không có marker nào active
        if (transform.childCount <= 1) return;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public RectTransform PlaceTimeMarker(float time, float ns)
    {
        float percentage = time / ns;

        float width = _rectTransform.rect.width;
        float localX = width * percentage;

        GameObject marker = Instantiate(MarkerPrefab, transform);

        marker.gameObject.SetActive(true);

        RectTransform markerRect = marker.GetComponent<RectTransform>();
        markerRect.anchoredPosition = new Vector2(-localX, 0f);

        return markerRect;
    }
}
