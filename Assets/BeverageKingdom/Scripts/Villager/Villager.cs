using UnityEngine;

public class Villager : MonoBehaviour
{
    public enum VillagerState
    {
        Idle,
        Walk,
        Attack,
        Dead
    }

    public float AttackRange = 2f;
    public float AttackCoolDown = 0.5f;
    float _coolDownTimer;

    public VillagerState currentState;

    private Vector3 targetPosition;
    bool moveOnSpawn = false;

    public VillagerMovement VillagerMovement;
    public VillagerAnimation VillagerAnimation;
    public Transform VillagerDetectionRange;
    public Transform VillagerCollision;
    bool IsDoneAttack = false;

    public float HP;

    public int Damage;

    public Animator animator;

    bool IsDead = false;

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
        animator.SetBool("Dead", index == 4);

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
        if (HP <= 0)
        {
            ChangeState(VillagerState.Dead);
        }
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
            case VillagerState.Dead:
                // VillagerMovement.SetStage(4);
                HandleDead();
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
        if (currentState == newState) return;

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

    void HandleDead()
    {
        if (IsDead != true)
        {
            IsDead = true;
            animator.Play("Dead", 0, 0f);
            Destroy(gameObject, 4f);
            Destroy(VillagerCollision.gameObject);
            Destroy(VillagerDetectionRange.gameObject);
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

                    if (enemy.CurrentHealth <= 0)
                    {
                        VillagerMovement.Target = null;
                        VillagerMovement.SetStage(1);
                    }
                }
            }
        }
    }

    void OnDoneAttack()
    {
        IsDoneAttack = true;
    }
}
