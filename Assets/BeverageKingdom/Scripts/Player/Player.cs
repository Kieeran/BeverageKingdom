using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [SerializeField] private WeaponController WeaponController;

    public static Player instance;

    [HideInInspector]
    InputMangager _inputMangager;

    public float moveSpeed;
    private bool facingRight = true;
    #region Component
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

    public JoystickMove JoystickMove;
    public float AttackCoolDown = 1f;
    float _coolDownTimer = 0f;

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
        _inputMangager = InputMangager.Instance;
    }
    void Update()
    {
        //Uncomment this line to use WSAD, keep comment to use joystick
        // SetVelocity(inputMangager.Horizontal * moveSpeed, inputMangager.Vertical * moveSpeed);
        // if (inputMangager.GetKeyToAttack())
        // {
        //     WeaponController.Attack();
        //     stateMachine.ChangeState(attack);
        // }
        // stateMachine.currentState.Update();
        // Flip();

        _coolDownTimer += Time.deltaTime;

        if (_coolDownTimer >= AttackCoolDown)
        {
            _coolDownTimer = 0f;

            WeaponController.Attack();
            stateMachine.ChangeState(attack);
            stateMachine.currentState.Update();
        }
    }

    ////Comment this line to use WSAD
    void FixedUpdate()
    {
        SetVelocity(JoystickMove.move.x * moveSpeed, JoystickMove.move.y * moveSpeed);
        Flip();
    }

    public void SetVelocity(float xInput, float yInput)
    {
        rb.velocity = new Vector2(xInput, yInput);
    }
    public void Flip()
    {
        //Uncomment this line to use WSAD, keep comment to use joystick
        // if ((rb.velocity.x > 0 && !facingRight) || (rb.velocity.x < 0 && facingRight))
        if ((JoystickMove.move.x > 0 && !facingRight) || (JoystickMove.move.x < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 s = transform.localScale;
            s.x *= -1;            // đảo ngược scale X
            transform.localScale = s;
        }
    }
}