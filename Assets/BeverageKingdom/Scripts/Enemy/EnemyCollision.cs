using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BB") && collision.name.Contains("BB"))
        {
            Destroy(transform.parent.gameObject);

            if (collision.name.Contains("Villager"))
            {
                if (collision.transform.parent.TryGetComponent<Villager>(out var villager))
                {
                    villager.DecreaseHP();
                }
            }
        }
    }
}
