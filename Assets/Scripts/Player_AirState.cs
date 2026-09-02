using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AirState : EntityState
{
    public Player_AirState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        //在空中能够水平移动
        if(player.MoveInput.x != 0)
        {
            player.SetVelocity(player.MoveInput.x * player.moveSpeed * player.inAirMoveMultiplier,rb.velocity.y);
        }
    }
}
