using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BB") && collision.name.Contains("BB"))
        {
            // Destroy(transform.parent.gameObject);

            if (collision.name.Contains("Villager"))
            {
                if (collision.transform.parent.TryGetComponent<Villager>(out var villager))
                {
                    // villager.DecreaseHP();
                }
            }
        }

        if (collision.CompareTag("BB") && collision.name.Contains("House"))
        {
            if (collision.transform.TryGetComponent<House>(out var house))
            {
                house.DecreaseHouseHP();
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
