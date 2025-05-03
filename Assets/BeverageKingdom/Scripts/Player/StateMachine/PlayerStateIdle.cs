using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerGroundState
{
    public PlayerStateIdle(PlayerStateMachine playerStateMachine, Player player, string namePlayerState) : base(playerStateMachine, player, namePlayerState)
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
        if (player.rb.velocity != Vector2.zero)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
