using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private WeaponController WeaponController;

    public static Player instance;

    public float moveSpeed;
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
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            WeaponController.Attack();
        }
        stateMachine.currentState.Update();
    }
    public void SetVelocity(float xInput, float yInput)
    {
        rb.velocity = new Vector2(xInput, yInput);
        //PlayerCtrl.Instance.flipController.CheckFlip();
    }
}
