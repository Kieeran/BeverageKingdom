using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateAttack : SkeletonState
{
    protected float distancePvsE;
    protected int dirAttack;
    public SkeletonStateAttack(Enemy _enemy, EnemyStateMachine _enemyStateMachine, string _animBollName, Skeleton _skeleton) : base(_enemy, _enemyStateMachine, _animBollName, _skeleton)
    {
    }

    public override void Enter()
    {   
        base.Enter();
        // skeleton.rb.velocity = Vector2.zero;
        GetDir();
        /*if (skeleton.enemyCtrl.flipController.facingDir != dirAttack)
            skeleton.enemyCtrl.flipController.FLip();*/
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
       /* if (skeleton.enemyCharStats.IsDead())
            skeleton.stateMachine.ChangeState(skeleton.skelStateDead);
        distancePvsE = skeleton.enemyCtrl.checkAttack.distance;*/
        
    }
    protected void GetDir()
    {
       /* if (PlayerCtrl.Instance.transform.position.x - skeleton.transform.position.x >= 0)
            dirAttack = 1;*/
        //else dirAttack = -1;
    }
}

