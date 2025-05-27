using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(EnemyEffect))]

public class Enemy : TriBehaviour
{
    [SerializeField] private EnemySO enemyData;
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

    public float maxHealth;
    [HideInInspector]
    public float CurrentHealth;
    public Animator animator;

    public float AttackRange;
    public float AttackCoolDown;
    float _coolDownTimer;
    public int Damage;

    bool IsDead = false;

    public Image HealthBarFillUI;

    private EnemyEffect _enemyEffect;

    protected override void Awake()
    {
        EnemyMovement.OnStageChange += SetAnimator;
        EnemyAnimation.OnDoneAttack += OnDoneAttack;
    }

    protected override void Start()
    {
        base.Start();

        ChangeState(EnemyState.Walk);
        Init();
        _enemyEffect = GetComponent<EnemyEffect>();
    }
    private void Init()
    {
        // Set default values if enemyData is null
        if (enemyData == null)
        {
            Debug.LogWarning($"EnemyData is null on {gameObject.name}, using default values");
            AttackRange = 2f;
            AttackCoolDown = 1f;
            Damage = 1;
            maxHealth = 6f;
        }
        else
        {
            AttackRange = enemyData.attackRange;
            AttackCoolDown = enemyData.attackCoolDown;
            Damage = enemyData.dameAttack;
            maxHealth = enemyData.maxHealth;
        }
        
        CurrentHealth = maxHealth;
        Debug.Log($"Enemy initialized with {maxHealth} max health");
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

        // Add debug log to check distance calculations
        float distance = Vector3.Distance(transform.position, EnemyMovement.Target.position);
        Debug.Log($"Distance to target: {distance}, Attack Range: {AttackRange}");
        
        // Changed to return true if distance is less than or equal to attack range
        return distance <= AttackRange;
    }

    void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;
        Debug.Log($"Enemy changing state from {currentState} to {newState}");
        currentState = newState;
    }

    void HandleIdle()
    {
        if (EnemyMovement.Target != null && EnemyMovement.IsEntityInRange)
        {
            EnemyMovement.Walk();
            ChangeState(EnemyState.Walk);
        }
        else
        {
            EnemyMovement.GoToKindom();
            ChangeState(EnemyState.Walk);
        }
    }

    void HandleWalk()
    {
        if (IsInAttackRange())
        {
            ChangeState(EnemyState.Attack);
            return;
        }

        if (EnemyMovement.Target == null)
        {
            EnemyMovement.GoToKindom();
        }
        else
        {
            EnemyMovement.Walk();
        }
    }

    public void HandleAttack()
    {
        if (!IsInAttackRange())
        {
            ChangeState(EnemyState.Walk);
            return;
        }

        EnemyMovement.SetStage(3);
        
        if (IsDoneAttack)
        {
            ApplyDamage();
            _coolDownTimer += Time.deltaTime;

            if (_coolDownTimer >= AttackCoolDown)
            {
                _coolDownTimer = 0;
                IsDoneAttack = false;
                // Only go to idle if we're no longer in attack range
                if (!IsInAttackRange())
                {
                    ChangeState(EnemyState.Walk);
                }
            }
        }
    }

    void HandleDead()
    {
        if (IsDead != true)
        {
            // EnemySpawner.Instance.NotifyEnemyKilled();
            IsDead = true;
            animator.Play("Dead", 0, 0f);
<<<<<<< Updated upstream
            Destroy(gameObject, 1f);
            Destroy(VillagerCollision.gameObject);
            Destroy(VillagerDetectionRange.gameObject);
=======
            if (VillagerCollision != null && VillagerCollision.gameObject != null)
                Destroy(VillagerCollision.gameObject);
            if (VillagerDetectionRange != null && VillagerDetectionRange.gameObject != null)
                Destroy(VillagerDetectionRange.gameObject);
            if (ItemSpawner != null)
                ItemSpawner.Spawn(transform.position, Quaternion.identity);
            Destroy(gameObject, 1f);
>>>>>>> Stashed changes
        }
    }

    void ApplyDamage()
    {
        if (_coolDownTimer > 0) return;

        var target = EnemyMovement.Target;
        if (target == null) return;

        // Try Player first since that's our primary target
        if (target.TryGetComponent<Player>(out var player))
        {
            Debug.Log($"Enemy attacking player for {Damage} damage");
            player.TakeDamage(Damage);
            if (player.HP <= 0)
            {
                EnemyMovement.Target = null;
                EnemyMovement.SetStage(1);
            }
        }
        // Fall back to villager if it's not a player
        else if (target.TryGetComponent<Villager>(out var villager))
        {
            Debug.Log($"Enemy attacking villager for {Damage} damage");
            villager.TakeDamage(Damage);
            if (villager.HP <= 0)
            {
                EnemyMovement.Target = null;
                EnemyMovement.SetStage(1);
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
        Debug.Log($"Enemy taking {amount} damage. Current health: {CurrentHealth}");
        CurrentHealth -= amount;
        HealthBarFillUI.DOFillAmount(CurrentHealth / maxHealth, 0.5f)
            .SetEase(Ease.OutBounce);

        if (_enemyEffect != null)
        {
            _enemyEffect.ApplyKnockBack();
            _enemyEffect.ApplyHitEffect();
        }
        
        Debug.Log($"Enemy health after damage: {CurrentHealth}");
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }    public void Die()
    {
        //Destroy(gameObject);
        // EnemySpawner.Instance.Despawm(transform);
        ChangeState(EnemyState.Dead);
    }
}
