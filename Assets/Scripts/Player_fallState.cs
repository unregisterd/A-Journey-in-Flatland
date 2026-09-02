using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_fallState : Player_AirState
{
    public Player_fallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        //如果碰到地面，切换为待机状态
        if (player.IsGrounded)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
