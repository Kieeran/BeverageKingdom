using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("House"))
        {
            if (collision.transform.TryGetComponent<House>(out var house))
            {
                house.DecreaseHouseHP();
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
