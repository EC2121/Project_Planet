using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WalkState :  Player_BaseState
{
    public Player_WalkState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(currentContext,playerStateFactory)
    {
        
    }
    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsWalkingHash,true);
        Context.Animator.SetBool(Context.IsRunningHash,false);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
       
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
        else if (Context.IsMovementPressed && Context.IsRunPressed)
        {
            SwitchState(Factory.Run());
        }
    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }
}