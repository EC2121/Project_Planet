using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Timers;
using UnityEngine;

public class Player_JumpAttack : Player_BaseState
{
   
    private float timer = 0.7f;

    public Player_JumpAttack(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        Context.Animator.SetBool("isJumpAttack", true);
        Context.Animator.SetBool(Context.IsJumpingHash, false);
        
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
        CheckSwitchStates();
        HandleGravity();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool("isJumpAttack", false);
        if (!Context.IsRunPressed)
        {
            Context.Animator.SetBool(Context.IsRunningHash, false);

            Context.AppliedMovementX = 0;
            Context.AppliedMovementZ = 0;
        }

        if (!Context.IsMovementPressed)
        {
            Context.Animator.SetBool(Context.IsRunningHash, false);
            Context.Animator.SetBool(Context.IsWalkingHash, false);
            Context.AppliedMovementX = 0; 
            Context.AppliedMovementZ = 0;
        }
    }

    public override void CheckSwitchStates()
    {
        if (Context.Hp <= 0)
        {
            SwitchState(Factory.Dead());
        }

        // if (Context.IsJumpPressed)
        // {
        //     SwitchState(Factory.Jump());
        // }
        if (Context.CharacterController.isGrounded && timer <=0)
            SwitchState(Factory.Grounded());
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
    public override void InitializeSubState()
    {
    }
}