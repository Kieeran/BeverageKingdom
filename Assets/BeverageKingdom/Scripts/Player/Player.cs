using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [SerializeField] private WeaponController WeaponController;

    public static Player instance;
    public InputMangager inputMangager;

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
    }
    void Update()
    {

        /*if (Joystick)
        {
            
        }*/

        SetVelocity(inputMangager.Horizontal * moveSpeed, inputMangager.Vertical * moveSpeed);

        if (inputMangager.GetKeyToAttack())
        {
            WeaponController.Attack();
            stateMachine.ChangeState(attack);
        }
        stateMachine.currentState.Update();
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

}
