using UnityEngine;

public class Villager : MonoBehaviour
{
    private Vector3 targetPosition;
    bool moveOnSpawn = false;

    public Movement VillagerMovement;

    public int HP = 3;

    public void DecreaseHP()
    {
        HP--;
        if (HP == 0) Destroy(gameObject);
    }

    public void SetTargetPosition(Vector3 pos)
    {
        targetPosition = pos;
        moveOnSpawn = true;
    }

    void Update()
    {
        if (moveOnSpawn == true)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                VillagerMovement.MoveSpeed * 5 * Time.deltaTime
            );
        }

        if (Vector3.Distance(transform.position, targetPosition) <= 0.01f && moveOnSpawn == true)
        {
            transform.position = targetPosition;
            moveOnSpawn = false;
        }
    }
}
