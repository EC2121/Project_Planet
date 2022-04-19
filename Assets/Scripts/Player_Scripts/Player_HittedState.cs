using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Player_HittedState : Player_BaseState
{
    private bool canExit = false;
    private float timer = 0.2f;

    public Player_HittedState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
   
    }

    public override void EnterState()
    {
        if (Context.Hp > 0)
        {
            Context.Hp -= 33f;
            Context.MaySliderValue = Context.Hp;
        }
        Context.Animator.SetBool(Context.IsHittedHash, true);
        Context.Animator.SetBool(Context.IsAttacking, false);
        Context.Animator.SetBool(Context.IsRunAttackingHash, false);


        // Context.Animator.SetBool(Context.IsWalkingHash,false);
        // Context.Animator.SetBool(Context.IsRunningHash,false);
        // Context.CurrentMovementX = 0;
        // Context.CurrentMovementZ = 0;
        canExit = true;
        Context.IsIsHitted = false;
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
        if (!Context.IsRunPressed)
        {
            Context.Animator.SetBool(Context.IsRunningHash, false);
            Context.AppliedMovementX = Context.CurrentMovementInput.x;
            Context.AppliedMovementZ = Context.CurrentMovementInput.y;
        }
        if (!Context.IsMovementPressed)
        {
            Context.Animator.SetBool(Context.IsRunningHash, false);
            Context.Animator.SetBool(Context.IsWalkingHash, false);
            Context.AppliedMovementX = 0f;
            Context.AppliedMovementZ = 0f;
        }
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsHittedHash, false);
       
        if (Context.IsIsHitted)
        {
            Context.RequireNewHit = true;
        }

        canExit = false;
    }

    public override void CheckSwitchStates()
    {
       
        // if (Context.IsMousePressed && Context.IsWeaponAttached && !Context.RequireNewAttack)
        // {
        //     SwitchState(Factory.StaffAttack());
        // }

        if (!Context.IsMovementPressed && Context.Animator.GetCurrentAnimatorStateInfo(0).IsName("Hitted"))
        {
            SwitchState(Factory.Idle());
        }
        if (Context.IsMovementPressed && !Context.IsRunPressed && !Context.HasBox && Context.Animator.GetCurrentAnimatorStateInfo(0).IsName("Hitted"))
        {
            SwitchState(Factory.Walk());
        }

        if (Context.IsMovementPressed && Context.IsRunPressed && Context.Animator.GetCurrentAnimatorStateInfo(0).IsName("Hitted"))
        {
            SwitchState(Factory.Run());
        }
    }

    public override void InitializeSubState()
    {
    }
}