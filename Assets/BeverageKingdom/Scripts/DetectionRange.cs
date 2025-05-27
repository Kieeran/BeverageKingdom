using System;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    public Action<Transform> OnInRange;
    public Action OnOutRange;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent == null) return;

        if (collision.transform.parent.CompareTag("Player") ||
        collision.transform.parent.CompareTag("Enemy") ||
        collision.transform.parent.CompareTag("Ally"))
        {
            OnInRange?.Invoke(collision.transform.parent);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent == null) return;

        if (collision.transform.parent.CompareTag("Player") ||
        collision.transform.parent.CompareTag("Enemy") ||
        collision.transform.parent.CompareTag("Ally"))
        {
            OnOutRange?.Invoke();
        }
    }
}