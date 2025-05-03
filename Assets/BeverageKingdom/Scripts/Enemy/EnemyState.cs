using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
    protected EnemyStateMachine enemyStateMachine;
    protected Enemy enemyBase;

    protected bool triggerCaller;
    private string animBoolName;
    protected float stateTimer;
    public EnemyState(Enemy _enemy, EnemyStateMachine _enemyStateMachine,string _animBollName)
    {
        this.enemyBase = _enemy;
        this.enemyStateMachine = _enemyStateMachine;
        this.animBoolName = _animBollName;
    }
    public virtual void Enter()
    {
        triggerCaller = false;
        // enemyBase.ani.SetBool(animBoolName, true);
    }
    public virtual void Update()
    {

    }
    public virtual void Exit()
    {
        // enemyBase.ani.SetBool(animBoolName, false);
    }
}
