using UnityEngine;

public class Villager : MonoBehaviour
{
    public enum VillagerState
    {
        Idle,
        Walk,
        Hit
    }

    public float AttackRange = 2f;
    public float AttackCoolDown = 0.5f;
    float _coolDownTimer;

    public VillagerState currentState;

    private Vector3 targetPosition;
    bool moveOnSpawn = false;

    public VillagerMovement VillagerMovement;
    public VillagerAnimation VillagerAnimation;
    bool IsDoneHit = false;

    public int HP = 3;

    public Animator animator;

    void Awake()
    {
        currentState = VillagerState.Idle;
        VillagerMovement.OnStageChange += SetAnimator;
        VillagerAnimation.OnDoneHit += OnDoneHit;
    }

    void SetAnimator(int index)
    {
        animator.SetBool("Idle", index == 1);
        animator.SetBool("Walk", index == 2);

        if (index == 3)
        {
            animator.SetBool("Hit", true);
            animator.Play("Hit", 0, 0f);
        }

        else
        {
            animator.SetBool("Hit", false);
        }
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

    bool IsInAttackRange()
    {
        if (VillagerMovement.Target == null) return false;

        float distance = Vector3.Distance(transform.position, VillagerMovement.Target.position);
        return distance >= 0 && distance <= AttackRange;
    }

    void Update()
    {
        switch (currentState)
        {
            case VillagerState.Idle:
                HandleIdle();
                break;

            case VillagerState.Walk:
                HandleWalk();
                break;

            case VillagerState.Hit:
                HandleHit();
                break;
        }

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

    void ChangeState(VillagerState newState)
    {
        currentState = newState;
    }

    void HandleIdle()
    {
        if (VillagerMovement.Target != null && VillagerMovement.IsEntityInRange == true)
        {
            ChangeState(VillagerState.Walk);
        }
    }

    void HandleWalk()
    {
        if (IsInAttackRange())
        {
            ChangeState(VillagerState.Hit);
        }

        else
        {
            if (VillagerMovement.Target == null)
            {
                ChangeState(VillagerState.Idle);
                return;
            }

            VillagerMovement.Walk();
        }
    }

    void HandleHit()
    {
        VillagerMovement.SetStage(3);
        if (IsDoneHit == true)
        {
            _coolDownTimer += Time.deltaTime;

            if (_coolDownTimer >= AttackCoolDown)
            {
                _coolDownTimer = 0;

                ChangeState(VillagerState.Idle);
                VillagerMovement.SetStage(1);
                IsDoneHit = false;
            }
        }
    }

    void OnDoneHit()
    {
        IsDoneHit = true;
    }
}
