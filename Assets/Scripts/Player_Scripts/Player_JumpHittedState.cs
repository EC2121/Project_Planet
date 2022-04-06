using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Player_JumpHittedState : Player_BaseState
{
    private bool canExit = false;
    private float timer = 0.2f;

    public Player_JumpHittedState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
        // Context.CurrentMovementY = Context.GroundGravity;
        // Context.AppliedMovementY = Context.GroundGravity;
    }

    public override void EnterState()
    {
        // Debug.Log("OH SI");
        if (Context.Hp > 0)
        {
            Context.Hp += 33f;
        }

        // Context.AttackCount = 0;
        Context.Animator.SetBool("isJumpHitted", true);
        Context.Animator.SetBool(Context.IsJumpingHash,false);
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

        HandleGravity();
        CheckSwitchStates();
    }

    void HandleGravity()
    {
        bool isFalling = Context.CurrentMovementY <= 0.0f || !Context.IsJumpPressed;
        float fallMultiplier = 2.0f;

        if (isFalling)
        {
            float previousY_Velocity = Context.CurrentMovementY;
            Context.CurrentMovementY = Context.CurrentMovementY +
                                       (Context.JumpGravities[Context.JumpCount] * fallMultiplier * Time.deltaTime);
            Context.AppliedMovementY = Mathf.Max((previousY_Velocity + Context.CurrentMovementY) * 0.5f, -20.0f);
        }
        else
        {
            float previousY_Velocity = Context.CurrentMovementY;
            Context.CurrentMovementY =
                Context.CurrentMovementY + (Context.JumpGravities[Context.JumpCount] * Time.deltaTime);
            Context.AppliedMovementY = (previousY_Velocity + Context.CurrentMovementY) * 0.5f;
        }
    }

    public override void ExitState()
    {
        Context.Animator.SetBool("isJumpHitted", false);
       // Context.AppliedMovementX = 0;
       // Context.AppliedMovementZ = 0;
       if (!Context.IsIsHitted)
       {
           if (!Context.IsRunPressed)
           {
               Context.Animator.SetBool(Context.IsRunningHash, false);

               Context.AppliedMovementX = 0;
               Context.AppliedMovementZ = 0;
           }

           if (!Context.IsMovementPressed)
           {
               Context.Animator.SetBool(Context.IsWalkingHash, false);
               Context.AppliedMovementX = 0;
               Context.AppliedMovementZ = 0;
           }

       }
      
       if (Context.IsJumpPressed)
       {
           Context.RequireNewJump = true;
       }

      // Context.CurrentJumpResetRoutine = Context.StartCoroutine(IJumpResetRoutine());
       if (Context.JumpCount == 3)
       {
           Context.JumpCount = 0;
           Context.Animator.SetInteger(Context.JumpCountHash, Context.JumpCount);
       }
        if (Context.IsIsHitted)
        {
            Context.RequireNewHit = true;
        }

        canExit = false;
    }

    public override void CheckSwitchStates()
    {
        if (Context.Hp <= 0)
        {
            SwitchState(Factory.Dead());
        }
        if (Context.CharacterController.isGrounded)
            SwitchState(Factory.Grounded());
        // if (Context.IsMousePressed && Context.IsWeaponAttached && !Context.RequireNewAttack && timer <= 0)
        // {
        //     SwitchState(Factory.StaffAttack());
        // }
        //
        // if (!Context.IsMovementPressed && timer <= 0)
        // {
        //     SwitchState(Factory.Idle());
        // }
        //
        // // if (Context.IsSwitchPressed && !Context.RequireNewWeaponSwitch && !Context.HasBox )
        // // {
        // //     SwitchState(Factory.SwitchWeapon());
        // // }
        // if (Context.IsMovementPressed && !Context.IsRunPressed && timer <= 0)
        // {
        //     SwitchState(Factory.Walk());
        // }
        //
        // if (!Context.HasBox && Context.IsRunPressed && timer <= 0)
        // {
        //     SwitchState(Factory.Run());
        // }
    }

    public override void InitializeSubState()
    {
    }
}