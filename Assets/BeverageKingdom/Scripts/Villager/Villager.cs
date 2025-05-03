using UnityEngine;

public class Villager : MonoBehaviour
{
    public enum VillagerState
    {
        Idle,
        Walk,
        Attack
    }

    public float AttackRange = 2f;
    public float AttackCoolDown = 0.5f;
    float _coolDownTimer;

    public VillagerState currentState;

    private Vector3 targetPosition;
    bool moveOnSpawn = false;

    public VillagerMovement VillagerMovement;
    public VillagerAnimation VillagerAnimation;
    bool IsDoneAttack = false;

    public float HP;

    public int Damage;

    public Animator animator;

    void Awake()
    {
        VillagerMovement.OnStageChange += SetAnimator;
        VillagerAnimation.OnDoneAttack += OnDoneAttack;
    }

    void Start()
    {
        ChangeState(VillagerState.Idle);
    }

    void SetAnimator(int index)
    {
        animator.SetBool("Idle", index == 1);
        animator.SetBool("Walk", index == 2);

        if (index == 3)
        {
            animator.SetBool("Attack", true);
            animator.Play("Attack", 0, 0f);
        }

        else
        {
            animator.SetBool("Attack", false);
        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0) Destroy(gameObject);
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
                VillagerMovement.SetStage(1);
                HandleIdle();
                break;

            case VillagerState.Walk:
                VillagerMovement.SetStage(2);
                HandleWalk();
                break;

            case VillagerState.Attack:
                VillagerMovement.SetStage(3);
                HandleAttack();
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
            ChangeState(VillagerState.Attack);
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

    void HandleAttack()
    {
        if (IsDoneAttack == true)
        {
            ApplyDamage();

            _coolDownTimer += Time.deltaTime;

            if (_coolDownTimer >= AttackCoolDown)
            {
                _coolDownTimer = 0;

                ChangeState(VillagerState.Idle);
                IsDoneAttack = false;
            }
        }
    }

    void ApplyDamage()
    {
        if (_coolDownTimer == 0)
        {
            if (VillagerMovement.Target != null)
            {
                if (VillagerMovement.Target.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.Deduct(Damage);
                }
            }
        }
    }

    void OnDoneAttack()
    {
        IsDoneAttack = true;
    }
}
