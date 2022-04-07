using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Interactable : Player_BaseState
{
    private bool canExit = false;
    public Player_Interactable(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
       // Context.Animator.SetBool(Context.IsRunningHash, false);
        //Context.Animator.SetBool(Context.IsWalkingHash,false);
        // if (Context.IsRunPressed)
        // {
        //     Context.Animator.SetBool(Context.IsRunningHash, false);
        //     //Context.Animator.SetBool(Context.IsWalkingHash,false);
        //     Context.AppliedMovementX = Context.CurrentMovementInput.x;
        //     Context.AppliedMovementZ = Context.CurrentMovementInput.y;
        // }
        GotBox();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
       
    }

    public override void ExitState()
    {
        canExit = false;
       
        // if (Context.IsMovementPressed)
        // {
        //     //Context.Animator.SetBool(Context.IsRunningHash, false);
        //     Context.Animator.SetBool(Context.IsWalkingHash,true);
        //     Context.AppliedMovementX = 0;
        //     Context.AppliedMovementZ =0;
        // }
        if (Context.IsInteract)
        {
            Context.RequireNewInteraction = true;
        }
    }

    public override void CheckSwitchStates()
    {
        // if (!Context.IsMovementPressed && canExit)
        // {
        //     SwitchState(Factory.Idle());
        // }
        // if ( Context.IsMovementPressed && canExit)
        // {
        //     SwitchState(Factory.Walk());
        // } 
        if (canExit)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
    }

    private void GotBox()
    {
        if (!Context.HasBox)
        {
            Context.HasBox = true;
            canExit = true;
            Context.Animator.SetBool(Context.HasBoxHash, true);
            Context.TakeTheBox.Invoke();
        }
        else if (Context.HasBox)
        {
            Context.HasBox = false;
            canExit = true;

            Context.Animator.SetBool(Context.HasBoxHash, false);
            Context.TakeTheBox.Invoke();
        }
    }
}