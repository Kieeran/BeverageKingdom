using System;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    public Action<Transform> OnInRange;
    public Action OnOutRange;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BB"))
        {
            Debug.Log("asdasd");

            OnInRange?.Invoke(collision.transform.parent);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Range"))
        {
            OnOutRange?.Invoke();
        }
    }
}
