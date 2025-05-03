using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateDead : SkeletonState
{
    public SkeletonStateDead(Enemy _enemy, EnemyStateMachine _enemyStateMachine, string _animBollName, Skeleton _skeleton) : base(_enemy, _enemyStateMachine, _animBollName, _skeleton)
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
    }
}
