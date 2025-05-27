using System;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    public Action<Transform> OnInRange;
    public Action OnOutRange;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Ally") ||
        collision.transform.parent.CompareTag("Enemy") ||
        collision.transform.parent.CompareTag("Player"))
        {
            OnInRange?.Invoke(collision.transform.parent);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Ally") ||
        collision.transform.parent.CompareTag("Enemy") ||
        collision.transform.parent.CompareTag("Player"))
        {
            OnOutRange?.Invoke();
        }
    }
}