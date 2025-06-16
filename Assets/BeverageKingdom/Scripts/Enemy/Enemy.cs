using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(EnemyEffect))]

public class Enemy : TriBehaviour
{
    [SerializeField] protected EnemySO enemyData;
    public enum EnemyState
    {
        Idle,
        Walk,
        Attack,
        Dead,
        Hit
    }
    public EnemyState currentState;

    public EnemyMovement EnemyMovement;
    public EnemyAnimation EnemyAnimation;
    public Transform EnemyDetectionRange;
    public Transform EnemyCollision;

    bool IsDoneAttack = false;

    [HideInInspector]
    public float MaxHealth;
    [HideInInspector]
    public float CurrentHealth;
    public Animator animator;
    public AnimatorOverrideController animatorOverride;

    [HideInInspector]
    public float AttackRange;
    [HideInInspector]
    public float AttackCoolDown;
    float _coolDownTimer;
    [HideInInspector]
    public int Damage;

    bool IsDead = false;

    public Image HealthBarFillUI;

    private EnemyEffect _enemyEffect;
    private ItemSpawner ItemSpawner;

    public Transform DetectionRangeVisual;
    public Transform BoundingBoxVisual;

    private SpriteRenderer _spriteRenderer;

    public float dropItemChance;

    protected override void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        DetectionRangeVisual.gameObject.SetActive(false);
        BoundingBoxVisual.gameObject.SetActive(false);

        Controller.Instance.VisualizeDetectionRange.OnValueChanged += UpdateVisualizeDetectionRange;
        Controller.Instance.VisualizeBoundingBox.OnValueChanged += UpdateVisualizeBoundingBox;
    }

    void UpdateVisualizeDetectionRange(bool oldVal, bool newVal)
    {
        if (DetectionRangeVisual != null && DetectionRangeVisual.gameObject != null)
            DetectionRangeVisual.gameObject.SetActive(newVal);
    }

    void UpdateVisualizeBoundingBox(bool oldVal, bool newVal)
    {
        if (BoundingBoxVisual != null && BoundingBoxVisual.gameObject != null)
            BoundingBoxVisual.gameObject.SetActive(newVal);
    }

    void OnDestroy()
    {
        Controller.Instance.VisualizeDetectionRange.OnValueChanged -= UpdateVisualizeDetectionRange;
        Controller.Instance.VisualizeBoundingBox.OnValueChanged -= UpdateVisualizeBoundingBox;
    }

    protected override void Start()
    {
        base.Start();
        EnemyMovement.OnStageChange += SetAnimator;
        EnemyAnimation.OnDoneAttack += OnDoneAttack;
        ChangeState(EnemyState.Walk);
        Init();
        _enemyEffect = GetComponent<EnemyEffect>();
        ItemSpawner = ItemSpawner.Instance;
    }

    public void SetEnemyData(EnemySO data)
    {
        enemyData = data;
        Init();
    }

    protected virtual void Init()
    {
        // Set default values if enemyData is null
        if (enemyData == null)
        {
            AttackRange = 2f;
            AttackCoolDown = 1f;
            Damage = 1;
            MaxHealth = 6f;
            if (EnemyMovement != null)
                EnemyMovement.MoveSpeed = 4f;

            // Default size and color
            transform.localScale = Vector3.one;
            if (_spriteRenderer != null)
                _spriteRenderer.color = Color.white;
        }
        else
        {
            AttackRange = enemyData.attackRange;
            AttackCoolDown = enemyData.attackCoolDown;
            Damage = enemyData.dameAttack;
            MaxHealth = enemyData.maxHealth;
            if (EnemyMovement != null)
                EnemyMovement.MoveSpeed = enemyData.speed;

            // Apply size and color from enemy data
            transform.localScale = enemyData.enemyScale;
            if (_spriteRenderer != null)
                _spriteRenderer.color = enemyData.enemyColor;
        }

        CurrentHealth = MaxHealth;
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
        ChangeState(EnemyState.Walk);
        if (EnemyMovement.Target != null && EnemyMovement.IsEntityInRange == true)
        {
            EnemyMovement.Walk();
        }

        else
        {
            EnemyMovement.GoToKindom();
        }
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
                IsDoneAttack = false;
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

            if (EnemyDetectionRange != null && EnemyDetectionRange.gameObject != null)
            {
                Destroy(EnemyDetectionRange.gameObject);
            }

            if (EnemyCollision != null && EnemyCollision.gameObject != null)
            {
                Destroy(EnemyCollision.gameObject);
            }

            if (ItemSpawner != null)
            {
                ItemSpawner.Spawn(dropItemChance, transform.position, Quaternion.identity);
            }
            Destroy(gameObject, 1f);
        }
    }

    void ApplyDamage()
    {
        if (_coolDownTimer == 0)
        {
            var target = EnemyMovement.Target;
            if (target != null)
            {
                if (target.TryGetComponent<Villager>(out var villager))
                {
                    villager.TakeDamage(Damage);

                    if (villager.HP <= 0)
                    {
                        EnemyMovement.Target = null;
                        EnemyMovement.SetStage(1);
                    }
                }
                if (target.TryGetComponent<Player>(out var player))
                {
                    player.TakeDamage(Damage);

                    if (player.HP <= 0)
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
        CurrentHealth = MaxHealth;
    }

    public void Deduct(int amount)
    {
        CurrentHealth -= amount;
        HealthBarFillUI.DOFillAmount(CurrentHealth / MaxHealth, 0.5f)
            .SetEase(Ease.OutBounce);

        if (_enemyEffect != null)
        {
            _enemyEffect.ApplyKnockBack();
            _enemyEffect.ApplyHitEffect();
        }

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        //Destroy(gameObject);
        // EnemySpawner.Instance.Despawm(transform);
        ChangeState(EnemyState.Dead);
    }
}