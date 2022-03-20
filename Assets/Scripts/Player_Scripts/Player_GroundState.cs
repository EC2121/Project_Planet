using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GroundState : Player_BaseState
{
    public Player_GroundState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(currentContext,playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState()
    {
        Context.CurrentMovementY = Context.GroundGravity;
        Context.CurrentRunMovementY = Context.GroundGravity;
        //Context.RequireJumpPress = false;
        //Context.RequireJumpPress = true;

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
        if (Context.IsJumpPressed)
        {
            SwitchState(Factory.Jump());
        }
    }

    public override void InitializeSubState()
    {
        if (!Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if (Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Run());
        }
    }
}
