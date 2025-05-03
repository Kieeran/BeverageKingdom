using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAttack : PlayerGroundState
{
    public PlayerStateAttack(PlayerStateMachine playerStateMachine, Player player, string namePlayerState) : base(playerStateMachine, player, namePlayerState)
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

}
