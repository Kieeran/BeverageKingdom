using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    Transform _leftTop;
    Transform _rightBottom;

    void Awake()
    {
        if (transform.childCount >= 2)
        {
            _leftTop = transform.GetChild(0);
            _rightBottom = transform.GetChild(1);
        }

        else
        {
            Debug.LogError("Not enough child objects. Make sure this object has at least 2 children.");
        }
    }

    public Vector2 GetRandomSpawnPos()
    {
        float minX = Mathf.Min(_leftTop.position.x, _rightBottom.position.x);
        float maxX = Mathf.Max(_leftTop.position.x, _rightBottom.position.x);
        float minY = Mathf.Min(_leftTop.position.y, _rightBottom.position.y);
        float maxY = Mathf.Max(_leftTop.position.y, _rightBottom.position.y);

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        return new Vector2(x, y);
    }
}
