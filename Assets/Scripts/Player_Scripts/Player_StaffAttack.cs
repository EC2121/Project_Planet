using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Player_StaffAttack : Player_BaseState
{
    private bool comboFailed = false;
    private float timer = 3f;

    public Player_StaffAttack(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
        Context.AttackId = 1;
    }


    public override void EnterState()
    {
        Context.Animator.SetInteger(Context.AttackIndexHash, 1);
        timer = 0.5f;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        timer -= Time.deltaTime;

        if (Context.AttackId > 3)
        {
            Context.AttackId = 0;
        }
        if (Context.IsMousePressed)
        {
            timer +=0.2f;
            Context.AttackId += 1;
            Context.Animator.SetInteger(Context.AttackIndexHash, Context.AttackId);
        }
        
    }

    public override void ExitState()
    {
        Context.Animator.SetInteger(Context.AttackIndexHash, 0);
        if (Context.IsMousePressed)
        {
            Context.RequireNewAttack = true;
        }
    }

    public override void CheckSwitchStates()
    {
        if (timer <= 0)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
    }
}