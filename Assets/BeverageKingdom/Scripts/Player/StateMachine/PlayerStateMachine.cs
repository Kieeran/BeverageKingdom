using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine 
{
    public PlayerState currentState { get; private set; }
    public void Initialize(PlayerState _StartState)
    {
        currentState = _StartState;
        currentState.Enter();


    }
    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
