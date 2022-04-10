using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Player_RunAttack : Player_BaseState
{
   
    private float timer = 0.5f;

    public Player_RunAttack(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsRunAttackingHash, true);
        
       // Context.IsIsHitted = false;
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsRunAttackingHash, false);
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

    public override void CheckSwitchStates()
    {
        if (Context.Hp <= 0)
        {
            SwitchState(Factory.Dead());
        }
        if (Context.CharacterController.isGrounded && timer <=0)
            SwitchState(Factory.Grounded());
    }

    public override void InitializeSubState()
    {
    }
}