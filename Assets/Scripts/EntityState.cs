using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState
{
    protected Player player;           // 引用玩家对象，以便控制移动、动画等
    protected StateMachine stateMachine; // 引用状态机，用于切换状态
    protected string animBoolName;      // 动画参数名（比如 "isMoving"）

    protected Animator anim;            // 快速访问玩家动画组件
    protected Rigidbody2D rb;            // 快速访问刚体
    protected float stateTimer;          // 可用于计时（如攻击持续时间）
    protected bool triggerCalled;        // 用于动画事件

    public EntityState(Player player,StateMachine stateMachine,string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;

        this.anim = player.Anim;
        this.rb = player.RB;
    }
    public virtual void Enter()//进入状态
    {
        //设置动画参数为true
        anim.SetBool(animBoolName,true);
        //重置动画触发器
        triggerCalled = false;
    }

    public virtual void Update()//更新状态
    {
        //每一帧的更新逻辑
        //包括状态计时、输入检测、状态切换等
    }

    public virtual void Exit()//退出状态
    {
        anim.SetBool(animBoolName,false);
        
    }

    // 这个方法将由 Player 的动画事件调用，通知状态动画播放完毕
    public void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
