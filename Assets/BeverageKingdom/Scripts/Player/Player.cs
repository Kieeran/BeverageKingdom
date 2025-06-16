using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform PlayerDetectionRange;
    public Transform PlayerCollision;
    [SerializeField] private WeaponController WeaponController;

    public static Player instance;
    public InputMangager inputMangager;

    public JoystickMove JoystickMove;
    // public bool UseJoystick;

    public float moveSpeed;
    private bool facingRight = true;
    #region Component
    public AnimatorOverrideController playerSword;
    public AnimatorOverrideController playerGun;
    [SerializeField] private Sprite playerSwordSpr;
    [SerializeField] private Sprite playerGunSpr;
    [SerializeField] private SpriteRenderer playerSpr;
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion
    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerStateIdle idleState { get; private set; }
    public PlayerStateMove moveState { get; private set; }
    public PlayerStateAttack attack { get; private set; }
    public PlayerStateDead dead { get; private set; }
    public PlayerStateHit hit { get; private set; }
    #endregion

    public Image HealthBarFillUI;

    public bool IsDead = false;
    public float MaxHP;
    public float HP;
    public float AttackCoolDown;
    float _coolDownTimer;

    public Action OnPlayerDead;

    private bool isShield;
    [SerializeField] private Transform shield;
    private bool isSpeed;

    Coroutine shieldCoroutine;

    public Transform DetectionRangeVisual;
    public Transform BoundingBoxVisual;

    public TMP_Text playerHPText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        stateMachine = new PlayerStateMachine();
        idleState = new PlayerStateIdle(stateMachine, this, "Idle");
        moveState = new PlayerStateMove(stateMachine, this, "Move");
        attack = new PlayerStateAttack(stateMachine, this, "Attack");
        dead = new PlayerStateDead(stateMachine, this, "Dead");
        hit = new PlayerStateHit(stateMachine, this, "Hit");

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

    private void Start()
    {
        isSpeed = false;
        anim = transform.Find("Model").GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initialize(idleState);
        inputMangager = InputMangager.Instance;

        MaxHP = HP;

        // _coolDownTimer = 0;
        _coolDownTimer = AttackCoolDown;
        isSpeed = false;
        isShield = false;
        // UIManager.Instance.MainCanvas.OnAttack += OnAttack;

        playerHPText.text = $"{HP}/{MaxHP}";
        playerHPText.gameObject.SetActive(false);
    }
    IEnumerator DisableAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        if (isShield)
        {
            shield.GetComponent<Animator>().SetBool("Break", true);
            StartCoroutine(DisableAfterDelay(shield.gameObject, 0.1f));

            ShieldOff();
            if (shieldCoroutine != null)
            {
                StopCoroutine(shieldCoroutine);
            }

            return;
        }

        HP -= damage;
        HealthBarFillUI.fillAmount = HP / MaxHP;

        stateMachine.ChangeState(hit);

        if (HP <= 0)
        {
            //anim.Play("Dead", 0, 0f);
            stateMachine.ChangeState(dead);
            rb.velocity = Vector2.zero;

            PlayerDetectionRange.gameObject.SetActive(false);
            PlayerCollision.gameObject.SetActive(false);
            WeaponController.gameObject.SetActive(false);
            IsDead = true;

            Invoke(nameof(OnPlayerAfterDead), 2f);
        }

        playerHPText.text = $"{HP}/{MaxHP}";
    }

    void OnPlayerAfterDead()
    {
        GameSystem.Instance.GameOver();
    }

    void OnAttack()
    {
        if (_coolDownTimer != 0) return;

        SoundManager.Instance?.PlaySound(SoundManager.Instance?.MeleeAttackSound, false);
        WeaponController.Attack();
        stateMachine.ChangeState(attack);
        stateMachine.currentState.Update();

        _coolDownTimer = AttackCoolDown;
    }

    void Update()
    {
        if (IsDead) return;

        // if (UseJoystick == false)
        // {
        //     SetVelocity(inputMangager.Horizontal * moveSpeed, inputMangager.Vertical * moveSpeed);
        //     Flip();
        // }

        // if (_coolDownTimer != 0)
        // {
        _coolDownTimer -= Time.deltaTime;

        if (_coolDownTimer <= 0)
        {
            // _coolDownTimer = 0;
            SoundManager.Instance?.PlaySound(SoundManager.Instance?.MeleeAttackSound, false);
            WeaponController.Attack();
            stateMachine.ChangeState(attack);
            stateMachine.currentState.Update();

            _coolDownTimer = AttackCoolDown;
        }
        // }

        // _coolDownTimer += Time.deltaTime;
        // if (_coolDownTimer >= AttackCoolDown)
        // {
        //     _coolDownTimer = 0;

        //     SoundManager.Instance?.PlaySound(SoundManager.Instance?.MeleeAttackSE, false);
        //     WeaponController.Attack();
        //     stateMachine.ChangeState(attack);
        //     stateMachine.currentState.Update();
        // }
    }

    void FixedUpdate()
    {
        if (IsDead) return;

        // if (UseJoystick == true)
        // {
        //     SetVelocity(JoystickMove.move.x * moveSpeed, JoystickMove.move.y * moveSpeed);
        //     Flip();
        // }

        Vector2 input;
        if (JoystickMove.move.sqrMagnitude > 0.01f)
        {
            input = JoystickMove.move;
        }
        else
        {
            input = new Vector2(inputMangager.Horizontal, inputMangager.Vertical);
        }

        SetVelocity(input.x * moveSpeed, input.y * moveSpeed);
        Flip();
    }

    public void SetVelocity(float xInput, float yInput)
    {
        rb.velocity = new Vector2(xInput, yInput);
        //PlayerCtrl.Instance.flipController.CheckFlip();
    }
    public void Flip()
    {
        if ((rb.velocity.x > 0 && !facingRight) || (rb.velocity.x < 0 && facingRight))
        {
            facingRight = !facingRight;
            Transform model = transform.Find("Model");
            float yRot = facingRight ? 0f : 180f;
            model.rotation = Quaternion.Euler(0f, yRot, 0f);
        }
    }
    public void SwapAnimatorController(int i)
    {
        if (i == 0)
        {
            anim.runtimeAnimatorController = playerSword;
            playerSpr.sprite = playerSwordSpr;
        }
        else if (i == 1)
        {
            anim.runtimeAnimatorController = playerGun;
            playerSpr.sprite = playerGunSpr;
        }
    }

    public void RecoverHp(float hp)
    {
        HP += hp;

        if (HP > MaxHP) HP = MaxHP;

        Debug.Log("Health + " + hp);
        HealthBarFillUI.fillAmount = HP / MaxHP;
    }

    public void ActiveShield(float buffDuration)
    {
        shieldCoroutine = StartCoroutine(StartShield(buffDuration));
    }

    public IEnumerator StartShield(float buffDuration)
    {
        if (isShield)
            yield break;
        ShieldOn();
        yield return new WaitForSeconds(buffDuration);
        ShieldOff();
        Debug.Log("shield");
    }

    void ShieldOn()
    {
        shield.gameObject.SetActive(true);
        isShield = true;
    }

    void ShieldOff()
    {
        shield.gameObject.SetActive(false);
        isShield = false;
    }

    public void SetSpeed(float addSpeed, float buffDuration)
    {
        StartCoroutine(StartSpeeding(addSpeed, buffDuration));
    }

    public IEnumerator StartSpeeding(float addSpeed, float buffDuration)
    {
        if (isSpeed)
            yield break;
        isSpeed = true;
        moveSpeed += addSpeed;
        yield return new WaitForSeconds(buffDuration);
        moveSpeed -= addSpeed;
        isSpeed = false;
    }
}