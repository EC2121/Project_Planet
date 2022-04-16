using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_StaffAttack : Player_BaseState
{
    private float timer = 0.565f;

    public Player_StaffAttack(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
   
    }
    
    public override void EnterState()
    {
        HandleCombo();
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsAttacking,false);
      
        if (!Context.IsRunPressed)
        {
            Context.Animator.SetBool(Context.IsRunningHash, false);
        
            Context.AppliedMovementX = Context.CurrentMovementInput.x;
            Context.AppliedMovementZ = Context.CurrentMovementInput.y;
        }
        
        if (!Context.IsMovementPressed)
        {
            Context.Animator.SetBool(Context.IsWalkingHash, false);
            Context.AppliedMovementX = 0;
            Context.AppliedMovementZ = 0;
        }
       
        if (Context.IsMousePressed)
        {
            Context.RequireNewAttack = true;
        }
        Context.CurrentAttackResetRoutine = Context.StartCoroutine(IAttackResetRoutine());
        if (Context.AttackCount == 4)
        {
            Context.AttackCount = 0;
            Context.Animator.SetInteger(Context.AttackIndexHash,Context.AttackCount);
        }
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsMovementPressed && timer <= 0)
        {
            SwitchState(Factory.Idle());
        }
        if ( Context.IsMovementPressed && timer <= 0)
        {
            SwitchState(Factory.Walk());
        } 
        if ( Context.IsJumpPressed && !Context.RequireNewJump  && timer <= 0)
        {
            SwitchState(Factory.Jump());
        }

        if (Context.IsIsHitted)
        {
            SwitchState(Factory.Hitted());
        }
    }

    void HandleCombo()
    {
        // if (Context.AttackCount < 4 && Context.CurrentAttackResetRoutine != null)
        // {
        //     Context.StopCoroutine(Context.CurrentAttackResetRoutine);
        // }
        // Context.Animator.SetBool(Context.IsAttacking,true);
        //
        // Context.AttackCount += 1;
        // Context.IsAttack = true;
        // Context.Animator.SetInteger(Context.AttackIndexHash, Context.AttackCount);
        
        if (Context.Animator.IsInTransition(3) || Context.Animator.GetCurrentAnimatorStateInfo(3).IsName("Ellen_Combo4")) return;
        
        if (Context.IsMousePressed && Context.Animator.GetCurrentAnimatorStateInfo(3).normalizedTime >= 0.3f)
        {
          
            timer = 0.8f;
            currentTime = Time.time;
        }
    }
    public override void InitializeSubState()
    {
    }
    
    IEnumerator IAttackResetRoutine()
    {
        yield return new WaitForSeconds(0.6f);
        Context.AttackCount = 0;
    }
}