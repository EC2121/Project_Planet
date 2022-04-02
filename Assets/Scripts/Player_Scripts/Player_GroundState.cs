using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player_GroundState : Player_BaseState
{
    public Player_GroundState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState()
    {
        Context.CurrentMovementY = Context.GroundGravity;
        Context.AppliedMovementY = Context.GroundGravity;
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
        if (Context.IsJumpPressed && !Context.RequireNewJump && !Context.HasBox)
        {
            SwitchState(Factory.Jump());
        }

        if (Context.IsInteract && ( !Context.IsWeaponAttached &&
                                   !Context.Animator.GetCurrentAnimatorStateInfo(1).IsName("Un_Equip") ) &&
                                    !Context.RequireNewWeaponSwitch && !Context.RequireNewInteraction &&
                                     Context.Mai_BoxIsTakable)

        {
            SwitchState(Factory.Interactable());
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
        else if (!Context.HasBox)
        {
            SetSubState(Factory.Run());
        }
    }

}
