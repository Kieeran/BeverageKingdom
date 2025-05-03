using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateRun : SkeletonState
{
    protected float runSpeed=5;
    protected float distancePvsE;
    protected int dirAttack;
    public SkeletonStateRun(Enemy _enemy, EnemyStateMachine _enemyStateMachine, string _animBollName, Skeleton _skeleton) : base(_enemy, _enemyStateMachine, _animBollName, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        GetDir();
        skeleton.SetMove(runSpeed);
        skeleton.FlipSkel();
       
       /* distancePvsE = skeleton.enemyCtrl.checkAttack.distance;
        if (distancePvsE <= 1.5)
            enemyStateMachine.ChangeState(skeleton.skelStateAttack);
        if (distancePvsE > 5)
            enemyStateMachine.ChangeState(skeleton.skelStateWalk);*/
    }
    protected void GetDir()
    {
      /*  if (PlayerCtrl.Instance.transform.position.x - skeleton.transform.position.x >= 0)
            dirAttack = 1;
        else dirAttack = -1;*/
       
    }
}
