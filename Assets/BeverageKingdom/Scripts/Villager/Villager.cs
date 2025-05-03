using UnityEngine;

public class Villager : MonoBehaviour
{
    private Vector3 targetPosition;
    bool moveOnSpawn = false;

    public VillagerMovement VillagerMovement;

    public int HP = 3;

    public Animator animator;

    void Awake()
    {
        VillagerMovement.OnStageChange += SetAnimator;
    }

    void SetAnimator(int index)
    {
        animator.SetBool("Idle", index == 1);
        animator.SetBool("Walk", index == 2);
        animator.SetBool("Hit", index == 3);
    }

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
                VillagerMovement.MoveSpeed * 4 * Time.deltaTime
            );
        }

        if (Vector3.Distance(transform.position, targetPosition) <= 0.01f && moveOnSpawn == true)
        {
            transform.position = targetPosition;
            moveOnSpawn = false;
        }
    }
}
