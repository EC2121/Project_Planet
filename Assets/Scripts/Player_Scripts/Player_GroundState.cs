using UnityEngine;

public class Player_GroundState : Player_BaseState
{
    public Player_GroundState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
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
        // if (Context.IsJumpPressed && !Context.RequireNewJump && !Context.HasBox && !Context.IsIsHitted)
        // {
        //     SwitchState(Factory.JumpHitted());
        // }
        if (Context.IsInteract && ( !Context.IsWeaponAttached && !Context.IsRunPressed &&
                                   !Context.Animator.GetCurrentAnimatorStateInfo(1).IsName(Context.UnEquipString) ) &&
            !Context.RequireNewWeaponSwitch && !Context.RequireNewInteraction &&
            Context.Mai_BoxIsTakable)
        {
            SwitchState(Factory.Interactable());
        }
        if (Context.Hp <= 0)
        {
            SwitchState(Factory.Dead());
        }
        // if (Context.IsIsHitted)
        // {
        //     SwitchState(Factory.Hitted());
        // }
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
        else if (!Context.HasBox && Context.IsMovementPressed && Context.IsRunPressed)
        {
            SetSubState(Factory.Run());
        }

        if (Context.IsSwitchPressed && !Context.RequireNewWeaponSwitch && !Context.HasBox)
        {
            SetSubState(Factory.SwitchWeapon());
        }

        if (Context.IsIsHitted)
        {
            SetSubState(Factory.Hitted());
        }
        //
        // if (Context.IsIsHitted && Context.IsJumpPressed)
        // {
        //     SetSubState(Factory.JumpHitted());
        // }
        if (Context.IsMousePressed && Context.IsWeaponAttached && !Context.RequireNewAttack)
        {
            SetSubState(Factory.StaffAttack());
        }

    }
}