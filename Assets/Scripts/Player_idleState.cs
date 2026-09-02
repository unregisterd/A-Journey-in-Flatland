using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_idleState : Player_GroundedState
{
    public Player_idleState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0,rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        //如果检测到水平输入不为0，则切换到移动状态
        if(player.MoveInput.x != 0)
        {
            stateMachine.ChangeState(player.MoveState);
        }
        
    }
}
