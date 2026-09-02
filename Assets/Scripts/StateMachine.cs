using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public EntityState currentState{get;private set;}

    public void Initialize(EntityState startingState)//初始化状态
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(EntityState newState)//改变状态
    {
        //避免切换到同一状态
        if(currentState == newState) return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }


}
