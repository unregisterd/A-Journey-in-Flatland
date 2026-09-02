using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GroundedState : EntityState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Update()
    {
        base.Update();

        if(Input.GetButtonDown("Jump") && player.IsGrounded)
        {
            stateMachine.ChangeState(player.JumpState);
        }
    }
}
