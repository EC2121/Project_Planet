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
        //IsRootState = true;
        //InitializeSubState();
    }

    public override void EnterState()
    {
        // Debug.Log("OH SI");
        if (Context.Hp > 0)
        {
            Context.Hp -= 33f;

        }
        // Context.AttackCount = 0;
        Context.Animator.SetBool("isHitted", true);
        // Context.Animator.SetBool(Context.IsWalkingHash, false);
        // Context.Animator.SetBool(Context.IsRunningHash, false);
        // Context.Animator.SetBool(Context.IsJumpingHash, false);
        // Context.CurrentMovementY = Context.GroundGravity;
        // Context.AppliedMovementY = Context.GroundGravity;
        // Context.AppliedMovementX = 0; 
        //Context.AppliedMovementZ = 0;
        // if (Context.IsMousePressed)
        // {
        //     Context.RequireNewAttack = true;
        //
        // }
        canExit = true;
        Context.IsIsHitted = false;
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
       
     
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool("isHitted", false);
       
        if (Context.IsIsHitted)
        {
            Context.RequireNewHit = true;
        }

        canExit = false;
    }

    public override void CheckSwitchStates()
    {
        // if(canExit  && timer <= 0)
        //   SwitchState(Factory.Grounded());
       
        if (Context.IsMousePressed && Context.IsWeaponAttached && !Context.RequireNewAttack && timer <= 0)
        {
            SwitchState(Factory.StaffAttack());
        }

        if (!Context.IsMovementPressed && timer <= 0)
        {
            SwitchState(Factory.Idle());
        }

        // if (Context.IsSwitchPressed && !Context.RequireNewWeaponSwitch && !Context.HasBox )
        // {
        //     SwitchState(Factory.SwitchWeapon());
        // }
        if (Context.IsMovementPressed && !Context.IsRunPressed && timer <= 0)
        {
            SwitchState(Factory.Walk());
        }

        if (!Context.HasBox && Context.IsRunPressed && timer <= 0)
        {
            SwitchState(Factory.Run());
        }
    }

    public override void InitializeSubState()
    {
    }
}