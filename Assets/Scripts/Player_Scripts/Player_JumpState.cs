using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpState : Player_BaseState
{
    private bool isFalling;

    public Player_JumpState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;

        InitializeSubState();
    }

    public override void EnterState()
    {
        isFalling = false;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleJump();
        HandleGravity();
        Debug.Log("ciao");
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsJumpingHash, false);
        Context.Animator.SetBool(Context.IsLandingHash, false);
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
        if (!isFalling && Context.IsJumpPressed)
        {
            isFalling = true;
            Context.Animator.SetBool(Context.IsJumpingHash, true);
            Context.CurrentMovementY += Context.JumpSpeed;
        }

        Context.Animator.SetFloat(Context.VelocityHash_Y, Context.CurrentMovementY / Context.JumpSpeed);

        if (isFalling && Context.CurrentMovementY < 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(Context.PlayerPos, Vector3.down, out hit, 0.5f, LayerMask.GetMask("Default")))
            {
                isFalling = false;
                Context.Animator.SetBool(Context.IsLandingHash, true);
                Context.RequireJumpPress = true;
            }
        }
    }

    void HandleGravity()
    {
        float gravity = -9.81f;
        Context.CurrentMovementY += gravity * Time.deltaTime;
    }
}