using System;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    public Action<Transform> OnInRange;
    public Action OnOutRange;

    void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("asdasd");

        if (collision.CompareTag("Range"))
        {
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
