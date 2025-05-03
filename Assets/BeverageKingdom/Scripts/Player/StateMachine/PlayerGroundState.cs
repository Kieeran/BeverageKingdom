using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerStateMachine playerStateMachine, Player player, string namePlayerState) : base(playerStateMachine, player, namePlayerState)
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
        
        if (InputMangager.Instance.GetKeyToAttack())
            stateMachine.ChangeState(player.attack);
        
    }
}
