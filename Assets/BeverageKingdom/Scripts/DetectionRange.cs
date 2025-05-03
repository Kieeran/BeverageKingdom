using System;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    public Action<Transform> OnInRange;
    public Action OnOutRange;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Range"))
        {
            OnInRange?.Invoke(collision.transform.parent);
            Debug.Log("In range with other entity");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Range"))
        {
            OnOutRange?.Invoke();
            Debug.Log("Out range with other entity");
        }
    }
}
