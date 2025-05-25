using System.Collections;
using System.Collections.Generic;
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
