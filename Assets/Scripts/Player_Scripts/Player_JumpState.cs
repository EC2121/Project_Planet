using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpState : Player_BaseState
{
    public Player_JumpState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;

        InitializeSubState();
    }

    public override void EnterState()
     {
        Context.Animator.SetBool(Context.IsAttacking,false);
        HandleJump();
    }

    public override void UpdateState()
    {  
        // Context.AppliedMovementX = Context.CameraMainTransform.forward.x;
        // Context.AppliedMovementZ = Context.CameraMainTransform.forward.z;
        CheckSwitchStates();
        HandleGravity();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsJumpingHash, false);

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

        if (Context.IsJumpPressed)
        {
            Context.RequireNewJump = true;
        }

        Context.CurrentJumpResetRoutine = Context.StartCoroutine(IJumpResetRoutine());
        if (Context.JumpCount == 3)
        {
            Context.JumpCount = 0;
            Context.Animator.SetInteger(Context.JumpCountHash, Context.JumpCount);
        }
    }

    public override void CheckSwitchStates()
    {
        if (Context.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
    }

    void HandleJump()
    {
        if (Context.JumpCount < 3 && Context.CurrentJumpResetRoutine != null)
        {
            Context.StopCoroutine(Context.CurrentJumpResetRoutine);
        }

        Context.Animator.SetBool(Context.IsJumpingHash, true);
        Context.IsJumping = true;
        Context.JumpCount += 1;
        Context.Animator.SetInteger(Context.JumpCountHash, Context.JumpCount);
        Context.CurrentMovementY = Context.InitialJumpVelocities[Context.JumpCount];
        Context.AppliedMovementY = Context.InitialJumpVelocities[Context.JumpCount];
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

    IEnumerator IJumpResetRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        Context.JumpCount = 0;
    }
}