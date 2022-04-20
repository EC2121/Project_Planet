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
        Context.Animator.SetBool(Context.IsJumpingHash, false);
        Context.CurrentMovementY = Context.GroundGravity;
        Context.AppliedMovementY = Context.GroundGravity;
        if (Context.IsMovementPressed && !Context.IsRunPressed)
        {
            Context.Animator.SetBool(Context.IsWalkingHash, true);
        }
        if (Context.IsMovementPressed && Context.IsRunPressed)
        {
            Context.Animator.SetBool(Context.IsWalkingHash, true);
            Context.Animator.SetBool(Context.IsRunningHash, true);

        }
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
        if (Context.IsInteract && ( !Context.IsWeaponAttached && !Context.IsRunPressed &&
                                    !Context.Animator.GetCurrentAnimatorStateInfo(1).IsName(Context.UnEquipString) ) &&
            !Context.RequireNewWeaponSwitch && !Context.RequireNewInteraction &&
           ( Context.Mai_BoxIsTakable || Context.IsCrystalActivable ))
        {
            SwitchState(Factory.Interactable());
        }
        if (Context.Hp <= 0)
        {
            SwitchState(Factory.Dead());
        }
    }

    public override void InitializeSubState()
    {
        if (!Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SetSubState(Factory.Idle());
        }
        if (Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }
        if (!Context.HasBox && Context.IsMovementPressed && Context.IsRunPressed)
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

        if (Context.IsMousePressed && !Context.IsJumpPressed && Context.IsWeaponAttached && Context.IsRunPressed)
        {
            SetSubState(Factory.RunAttack());
        }
        if (Context.IsMousePressed && Context.IsWeaponAttached && !Context.RequireNewAttack)
        {
            SetSubState(Factory.StaffAttack());
        }
    }
}