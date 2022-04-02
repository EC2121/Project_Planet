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
    }

    public override void EnterState()
    {
        if (Context.Mai_BoxIsTakable)
            Context.Animator.SetBool(Context.IsRunningHash, false);
        // Context.Animator.SetBool(Context.IsWalkingHash,false);

    }

    public override void UpdateState()
    {
        GotBox();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        canExit = false;
        if (Context.IsInteract)
        {
            Context.RequireNewInteraction = true;
        }
    }

    public override void CheckSwitchStates()
    {
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