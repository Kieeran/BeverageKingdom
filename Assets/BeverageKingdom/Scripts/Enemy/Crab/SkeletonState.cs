using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonState : EnemyState
{
    protected Skeleton skeleton;
    public SkeletonState(Enemy _enemy, EnemyStateMachine _enemyStateMachine, string _animBollName,Skeleton _skeleton) : base(_enemy, _enemyStateMachine, _animBollName)
    {
        this.skeleton = _skeleton;
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
    }
}
