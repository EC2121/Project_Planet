using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Player_RunAttack : Player_BaseState
{
   
    private float timer = 0.7413793f;

    public Player_RunAttack(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsRunAttackingHash, true);
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsRunAttackingHash, false);
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
            Context.AppliedMovementX = 0;
            Context.AppliedMovementZ = 0;
        }
        if (Context.IsMousePressed)
        {
            Context.RequireNewAttack = true;
        }
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
        if (!Context.IsMovementPressed && Context.Animator.GetCurrentAnimatorStateInfo(0).IsName("RunAttack"))
        {
            SwitchState(Factory.Idle());
        }
        if (Context.IsMovementPressed && !Context.IsRunPressed && Context.Animator.GetCurrentAnimatorStateInfo(0).IsName("RunAttack"))
        {
            SwitchState(Factory.Walk());
        }
        if (Context.IsMovementPressed && Context.IsRunPressed && Context.Animator.GetCurrentAnimatorStateInfo(0).IsName("RunAttack"))
        {
            SwitchState(Factory.Run());
        }
    }

    public override void InitializeSubState()
    {
    }
}