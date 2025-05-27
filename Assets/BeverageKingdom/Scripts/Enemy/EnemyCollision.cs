using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private Enemy enemyComponent;
    private bool hasHitHouse = false;

    private void Start()
    {
        enemyComponent = GetComponentInParent<Enemy>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if (hasHitHouse) return; // Prevent multiple house hits

        // if (collision.CompareTag("BB") && collision.name.Contains("BB"))
        // {
        //     if (collision.name.Contains("Villager"))
        //     {
        //         if (collision.transform.parent.TryGetComponent<Villager>(out var villager))
        //         {
        //             // villager.DecreaseHP();
        //         }
        //     }
        // }

        // if (collision.CompareTag("BB") && collision.name.Contains("House"))
        // if (collision.name.Contains("House"))
        // {
        //     if (collision.transform.TryGetComponent<House>(out var house))
        //     {
        //         hasHitHouse = true;
        //         house.DecreaseHouseHP();

        //         // If the enemy is still alive, destroy it
        //         if (enemyComponent != null)
        //         {
        //             enemyComponent.Die();
        //         }
        //         else
        //         {
        //             Destroy(transform.parent.gameObject);
        //         }
        //     }
        // }
    }
}
