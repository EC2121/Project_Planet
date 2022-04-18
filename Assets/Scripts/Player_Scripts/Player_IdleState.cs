using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_IdleState : Player_BaseState
{
    public Player_IdleState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsWalkingHash, false);
        Context.Animator.SetBool(Context.IsRunningHash, false);
        Context.Animator.SetBool(Context.IsHittedHash, false);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.AppliedMovementX = 0;
        Context.AppliedMovementZ = 0;
    }

    public override void CheckSwitchStates()
    {
        if (Context.Hp <= 0)
        {
            SwitchState(Factory.Dead());
        }

        if (Context.IsIsHitted)
        {
            SwitchState(Factory.Hitted());
        }

        if (Context.IsMousePressed && Context.IsWeaponAttached && !Context.RequireNewAttack)
        {
            SwitchState(Factory.StaffAttack());
        }

        if (Context.IsMovementPressed && Context.IsRunPressed && !Context.HasBox)
        {
            SwitchState(Factory.Run());
        }
         if (Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SwitchState(Factory.Walk());
        }

        if (Context.IsSwitchPressed && !Context.RequireNewWeaponSwitch && !Context.HasBox)
        {
            SwitchState(Factory.SwitchWeapon());
        }
    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }
}