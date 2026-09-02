using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_moveState : Player_GroundedState
{
    public Player_moveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Update()
    {
        base.Update();  

        player.SetVelocity(player.MoveInput.x * player.moveSpeed,rb.velocity.y);
        
        if(player.MoveInput.x == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }     
    }
}
