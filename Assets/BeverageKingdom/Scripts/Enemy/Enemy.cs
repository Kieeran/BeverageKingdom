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
    public Transform VillagerDetectionRange;
    public Transform VillagerCollision;

    bool IsDoneAttack = false;

    [SerializeField] private int maxHealth = 10;
    public int CurrentHealth;
    public Animator animator;

    public float AttackRange;
    public float AttackCoolDown;
    float _coolDownTimer;

    public int Damage;
    bool IsDead = false;

    protected override void Awake()
    {
        CurrentHealth = maxHealth;

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
                // VillagerMovement.SetStage(4);
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

    public void HandleAttack()
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
            if (EnemyMovement.Target != null)
            {
                if (EnemyMovement.Target.TryGetComponent<Villager>(out var villager))
                {
                    villager.TakeDamage(Damage);

                    if (villager.HP <= 0)
                    {
                        EnemyMovement.Target = null;
                        EnemyMovement.SetStage(1);
                    }
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
        CurrentHealth = maxHealth;
    }

    public void Deduct(int amount)
    {
        CurrentHealth -= amount;

        EnemyEffect effect = GetComponent<EnemyEffect>();
        if (effect != null)
        {
            effect.ApplyKnockBack();
        }

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Destroy(gameObject);
        // EnemySpawner.Instance.Despawm(transform);
        ChangeState(EnemyState.Dead);
    }
}
