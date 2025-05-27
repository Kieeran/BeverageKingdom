using System;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    public Action<Transform> OnInRange;
    public Action OnOutRange;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("Ally") ||
        collision.transform.root.CompareTag("Enemy") ||
        collision.transform.root.CompareTag("Player"))
        {
            OnInRange?.Invoke(collision.transform.parent);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("Ally") ||
        collision.transform.root.CompareTag("Enemy") ||
        collision.transform.root.CompareTag("Player"))
        {
            OnOutRange?.Invoke();
        }
    }
}