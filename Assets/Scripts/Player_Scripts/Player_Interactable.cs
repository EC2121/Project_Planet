using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Interactable : Player_BaseState
{

    private bool canExit = false;
    private float timer = 0f;
    public Player_Interactable(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Interact();
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
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
        if (canExit && timer >= 1)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
    }

    private void Interact()
    {
        if (Context.Mai_BoxIsTakable)
        {
            Context.GamePlayerFinalePhase.Invoke();
            if (!Context.HasBox)
            {
                Context.HasBox = true;
                timer = 1;
                canExit = true;
                Context.Animator.SetBool(Context.HasBoxHash, true);
                Context.TakeTheBox.Invoke();
            }
            else if (Context.HasBox)
            {
                Context.HasBox = false;
                timer = 1;
                canExit = true;
                Context.Animator.SetBool(Context.HasBoxHash, false);
                Context.TakeTheBox.Invoke();
            }
        }
        if (Context.IsCrystalActivable)
        {
            Context.CanCrystal.Invoke();
            timer = 1;
            canExit = true;

        }
        //if (Context.CanReviveRoby)
        //{
        //    Context.ReviveSliderValue = timer;
        //    if (timer > 1)
        //    {
        //        Context.ReviveRoby.Invoke();
        //    }
        //    canExit = true;
        //}
    }

}