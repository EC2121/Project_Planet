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
        //Context.Animator.SetBool(Context.IsAttacking,false);
        Context.Animator.SetBool(Context.IsRunAttackingHash, false);

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Context.AppliedMovementX = Context.CurrentMovementInput.x;
        Context.AppliedMovementZ = Context.CurrentMovementInput.y;
    }

    public override void ExitState()
    {
  
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
        if (Context.IsSwitchPressed && !Context.RequireNewWeaponSwitch && !Context.HasBox)
        {
            SwitchState(Factory.SwitchWeapon());
        }
        if (Context.IsMousePressed && Context.IsWeaponAttached && !Context.RequireNewAttack)
        {
            SwitchState(Factory.StaffAttack());
        }
        if (!Context.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
        else if (Context.IsMovementPressed && Context.IsRunPressed && !Context.HasBox)
        {
            SwitchState(Factory.Run());
        }
    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }
    
}
