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
        //Context.CurrentMovementX = 0f;
        //Context.AppliedMovementX = 0f;
        //Context.CurrentMovementZ = 0f;
        //Context.AppliedMovementZ = 0f;
        //Context.AppliedMovementX = 0;
        //Context.AppliedMovementZ = 0;
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
           ( Context.Mai_BoxIsTakable || Context.IsCrystalActivable || Context.CanReviveRoby))
        {
            SwitchState(Factory.Interactable());
        }
        //Debug.Log(Context.Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));
        //if (Context.IsMousePressed && (Context.Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || Context.Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump 2")) && Context.Animator.IsInTransition(0))
        //{
        //    Debug.Log("CEPROVOOOO");
        //    SwitchState(Factory.JumpAttack());
        //}
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
        //if (Context.IsMousePressed && !Context.RequireNewAttack && Context.IsWeaponAttached && !Context.CharacterController.isGrounded &&
        //    Context.JumpCount <= 2)
        //{
        //    Debug.Log("CEPROVOOOO");
        //    SetSubState(Factory.JumpAttack());
        //}
    }
}