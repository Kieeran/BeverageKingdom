using UnityEngine;

public class Enemy : TriBehaviour
{
    public enum EnemyState
    {
        Idle,
        Walk,
        Attack,
        Dead
    }
    public EnemyState currentState;

    public EnemyMovement EnemyMovement;
    public EnemyAnimation EnemyAnimation;

    bool IsDoneAttack = false;

    public float Damage;
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;
    public Animator animator;

    public float AttackRange;
    public float AttackCoolDown;
    float _coolDownTimer;

    bool IsDead = false;

    protected override void Awake()
    {
        currentHealth = maxHealth;

        EnemyMovement.OnStageChange += SetAnimator;
        EnemyAnimation.OnDoneAttack += OnDoneAttack;
    }

    protected override void Start()
    {
        base.Start();

        ChangeState(EnemyState.Walk);
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

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                EnemyMovement.SetStage(1);
                HandleIdle();
                break;

            case EnemyState.Walk:
                EnemyMovement.SetStage(2);
                HandleWalk();
                break;

            case EnemyState.Attack:
                EnemyMovement.SetStage(3);
                HandleAttack();
                break;
            case EnemyState.Dead:
                // EnemyMovement.SetStage(4);
                HandleDead();
                break;
        }
    }

    bool IsInAttackRange()
    {
        if (EnemyMovement.Target == null) return false;

        float distance = Vector3.Distance(transform.position, EnemyMovement.Target.position);
        return distance >= 0 && distance <= AttackRange;
    }

    void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }

    void HandleIdle()
    {
        if (EnemyMovement.Target != null && EnemyMovement.IsEntityInRange == true)
        {
            EnemyMovement.Walk();
        }

        else
        {
            EnemyMovement.GoToKindom();
        }

        ChangeState(EnemyState.Walk);
    }

    void HandleWalk()
    {
        if (IsInAttackRange())
        {
            ChangeState(EnemyState.Attack);
        }

        else
        {
            if (EnemyMovement.Target == null)
            {
                EnemyMovement.GoToKindom();
            }

            else
            {
                EnemyMovement.Walk();
            }
        }
    }

    void HandleAttack()
    {
        EnemyMovement.SetStage(3);
        if (IsDoneAttack == true)
        {
            ApplyDamage();
            _coolDownTimer += Time.deltaTime;

            if (_coolDownTimer >= AttackCoolDown)
            {
                _coolDownTimer = 0;

                ChangeState(EnemyState.Idle);
                EnemyMovement.SetStage(1);
                IsDoneAttack = false;
            }
        }
    }

    void HandleDead()
    {
        if (IsDead != true)
        {
            animator.Play("Dead", 0, 0f);
            IsDead = true;
            Destroy(gameObject, 4f);
        }
    }

    void ApplyDamage()
    {
        if (_coolDownTimer == 0)
        {
            if (EnemyMovement.Target != null)
            {
                if (EnemyMovement.Target.TryGetComponent<Villager>(out var villager))
                {
                    villager.TakeDamage(Damage);
                }
            }
        }
    }

    void OnDoneAttack()
    {
        IsDoneAttack = true;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        currentHealth = maxHealth;
    }

    public void Deduct(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ChangeState(EnemyState.Dead);
    }
}
