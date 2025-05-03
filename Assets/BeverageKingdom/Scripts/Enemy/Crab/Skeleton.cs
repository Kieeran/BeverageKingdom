using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Skeleton : Enemy
{
    public EnemyStateMachine stateMachine { get; private set; }
    public SkeletonStateDead skelStateDead { get; private set; }
    public SkeletonStateAttack skelStateAttack { get; private set; }
    public SkeletonStateRun skelStateRun { get; private set; }
    public skelStateHit skelStateHit { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        skelStateDead = new SkeletonStateDead(this, stateMachine, "Dead",this);
        skelStateAttack = new SkeletonStateAttack(this, stateMachine, "Attack",this);
        skelStateRun = new SkeletonStateRun(this, stateMachine, "Run", this);
       // skelStateHit = new skelStateHit(this, stateMachine, "Hit", this);
        stateMachine.Initialize(skelStateRun);
    }
    private void Update()
    {
        stateMachine.curentState.Update();
    }
    public void SetMove(float xInput)
    {
       // rb.velocity = new Vector2(xInput * enemyCtrl.flipController.facingDir, rb.velocity.y);
    }
    public void FlipSkel()
    {
       // enemyCtrl.flipController.CheckFlip();
    }
    public void SkelDeadAnimaTion()
    {
        stateMachine.ChangeState(skelStateDead);
    }
    public void SkelDead()
    {
        //enemyCtrl.itemDrop.GenerateDrop();
        Destroy(gameObject);
        // after i will do desapawn
    }
}
