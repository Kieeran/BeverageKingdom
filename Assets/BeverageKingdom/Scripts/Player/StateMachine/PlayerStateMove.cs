using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMove : PlayerGroundState
{
    public PlayerStateMove(PlayerStateMachine playerStateMachine, Player player, string namePlayerState) : base(playerStateMachine, player, namePlayerState)
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
        player.SetVelocity( xInput* player.moveSpeed , rb.velocity.y);
        if(player.rb.velocity == Vector2.zero)
        {
            stateMachine.ChangeState(player.idleState);
        }
        /*if (player.rb.velocity.x > 0)
        {
            player.Flip();
        }
        else
        {
            player.Flip();
        }*/
    }
}
