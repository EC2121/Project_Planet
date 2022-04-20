using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpState : Player_BaseState
{
    private bool isBeenHitted = false;
    private bool isJumpAttacked = false;
    private float timer = 0f;

    public Player_JumpState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;

        InitializeSubState();
    }

    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsAttacking, false);
        Context.Animator.SetBool(Context.IsHittedHash, false);
        Context.Animator.SetBool(Context.IsRunAttackingHash, false);
        Context.RequireNewAttack = false;
        isBeenHitted = false;

        HandleJump();
        SetJumpHeightRun();

    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;

        CheckSwitchStates();
        HandleGravity();
        HandleAnimation();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsJumpingHash, false);
        Context.Animator.SetBool(Context.IsJumpHittedHash, false);
        Context.Animator.SetBool(Context.IsJumpAttackHash, false);
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

        if (Context.IsJumpPressed)
        {
            Context.RequireNewJump = true;
        }

        if (Context.IsMousePressed)
        {
            Context.RequireNewAttack = true;
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

     
        if (Context.CharacterController.isGrounded && (Context.Animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack") || !Context.Animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack")))
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

    void HandleAnimation()
    {
        if (Context.IsIsHitted && !isBeenHitted)
        {
            isBeenHitted = true;
            Context.IsIsHitted = false;

            Context.Animator.SetBool(Context.IsJumpHittedHash, true);
            Context.Hp -= 30f;
            Context.MaySliderValue = Context.Hp;
        }

        if (Context.IsMousePressed && Context.IsWeaponAttached && !Context.CharacterController.isGrounded &&
            !isJumpAttacked && Context.JumpCount <= 2)
        {
            isJumpAttacked = true;
            
            Context.Animator.SetBool(Context.IsJumpAttackHash, true);
        }
    }

    void HandleGravity()
    {
        bool isFalling = Context.CurrentMovementY <= 0f || !Context.IsJumpPressed;
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

    void SetJumpHeightRun()
    {
        if (Context.IsMovementPressed && !Context.IsRunPressed)
        {
            Context.CurrentMovementX = Context.CurrentMovementInput.x * 2.5f;
            Context.CurrentMovementZ = Context.CurrentMovementInput.y * 2.5f;
        }

        if (Context.IsMovementPressed && Context.IsRunPressed)
        {
            Context.CurrentMovementX = Context.CurrentMovementInput.x * 4;
            Context.CurrentMovementZ = Context.CurrentMovementInput.y * 4;
        }
    }

    IEnumerator IJumpResetRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        Context.JumpCount = 0;
    }
}