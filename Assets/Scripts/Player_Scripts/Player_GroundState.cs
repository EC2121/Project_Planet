using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

        if (Context.IsSwitchPressed && !Context.RequireNewWeaponSwitch)
        {
            SwitchState(Factory.SwitchWeapon());
        }
        
        if (Context.IsMousePressed && Context.IsWeaponAttached)
        {
            SwitchState(Factory.StaffAttack());
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
    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(2f);
        Context.AttackId = 0;
    }
}
