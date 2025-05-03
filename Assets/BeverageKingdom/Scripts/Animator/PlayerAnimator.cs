using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Player player;
    private void Start()
    {
        player = Player.instance;
    }
    public void EndAttack()
    {
        player.stateMachine.ChangeState(player.idleState);

         
    }
}
