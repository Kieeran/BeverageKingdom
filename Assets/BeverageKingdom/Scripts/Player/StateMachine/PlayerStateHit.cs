using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHit : PlayerGroundState
{
    public PlayerStateHit(PlayerStateMachine playerStateMachine, Player player, string namePlayerState) : base(playerStateMachine, player, namePlayerState)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // Add any additional logic for entering the hit state here
    }

    public override void Exit()
    {
        base.Exit();
        // Add any additional logic for exiting the hit state here
    }
}
