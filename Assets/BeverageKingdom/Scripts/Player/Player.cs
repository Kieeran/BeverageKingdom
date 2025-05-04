using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    public Transform PlayerDetectionRange;
    public Transform PlayerCollision;
    [SerializeField] private WeaponController WeaponController;

    public static Player instance;
    public InputMangager inputMangager;

    public JoystickMove JoystickMove;
    public bool UseJoystick;

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
    [HideInInspector]
    public float MaxHP;
    public float HP;
    public float AttackCoolDown;
    float _coolDownTimer;

    public Action OnPlayerDead;

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

    }

    private void Start()
    {
        anim = transform.Find("Model").GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initialize(idleState);
        inputMangager = InputMangager.Instance;

        MaxHP = HP;
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        HealthBarFillUI.fillAmount = HP / MaxHP;

        if (HP <= 0)
        {
            //anim.Play("Dead", 0, 0f);
            stateMachine.ChangeState(dead);

            PlayerDetectionRange.gameObject.SetActive(false);
            PlayerCollision.gameObject.SetActive(false);
            WeaponController.gameObject.SetActive(false);
            IsDead = true;

            Invoke("OnPlayerAfterDead", 2f);
        }
    }

    void OnPlayerAfterDead()
    {
        Time.timeScale = 0f;
        GameSystem.instance.GameOver();
        // OnPlayerDead?.Invoke();
    }

    void Update()
    {
        if (IsDead) return;

        if (UseJoystick == false)
        {
            SetVelocity(inputMangager.Horizontal * moveSpeed, inputMangager.Vertical * moveSpeed);
            Flip();
        }

        _coolDownTimer += Time.deltaTime;
        if (_coolDownTimer >= AttackCoolDown)
        {
            _coolDownTimer = 0;

            WeaponController.Attack();
            stateMachine.ChangeState(attack);
            stateMachine.currentState.Update();
        }
    }

    void FixedUpdate()
    {
        if (IsDead) return;

        if (UseJoystick == true)
        {
            SetVelocity(JoystickMove.move.x * moveSpeed, JoystickMove.move.y * moveSpeed);
            Flip();
        }
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
    public void LevelUp()
    {
        MaxHP += 5;
        HP = MaxHP;
        Debug.Log("Level up");
        HealthBarFillUI.fillAmount = HP / MaxHP;
    }
}