using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState curentState { get; private set; }
    public void Initialize(EnemyState _startState)
    {
        curentState = _startState;
        curentState.Enter();
    }
    public void ChangeState(EnemyState _newState)
    {
        curentState.Exit();
        curentState = _newState;
        curentState.Enter();
     }
}
