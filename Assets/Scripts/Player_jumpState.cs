using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_jumpState : Player_AirState
{
    public bool IsActive{ get; private set; }
    public Player_jumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        IsActive = true;
        //进入跳跃状态时需要让物体上升
        player.SetVelocity(rb.velocity.x,player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if(rb.velocity.y < 0)
        {
            IsActive = false;
            stateMachine.ChangeState(player.fallState);
        }
    }
}
