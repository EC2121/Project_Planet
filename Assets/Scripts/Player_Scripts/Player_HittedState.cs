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
   
    }

    public override void EnterState()
    {
        if (Context.Hp > 0)
        {
            Context.Hp -= 33f;
            Context.MaySliderValue = Context.Hp;
        }
        Context.Animator.SetBool(Context.IsHittedHash, true);
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
        Context.Animator.SetBool(Context.IsHittedHash, false);
       
        if (Context.IsIsHitted)
        {
            Context.RequireNewHit = true;
        }

        canExit = false;
    }

    public override void CheckSwitchStates()
    {
       
        if (Context.IsMousePressed && Context.IsWeaponAttached && !Context.RequireNewAttack && timer <= 0)
        {
            SwitchState(Factory.StaffAttack());
        }

        if (!Context.IsMovementPressed && timer <= 0)
        {
            SwitchState(Factory.Idle());
        }
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