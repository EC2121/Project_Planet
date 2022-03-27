using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_RunState : Player_BaseState
{
    public Player_RunState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(currentContext,playerStateFactory)
    {
        
    }
    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsWalkingHash,true);
        Context.Animator.SetBool(Context.IsRunningHash,true);
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
        else if (Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SwitchState(Factory.Walk());
        }
    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }
}
