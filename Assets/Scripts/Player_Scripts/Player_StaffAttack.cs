using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_StaffAttack : Player_BaseState
{
    private float timer;
    private float currentTime = 0;
    private int attackIndex;

    public Player_StaffAttack(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {

    }

    public override void EnterState()
    {
        timer = 0.35f;
        Context.Animator.SetBool(Context.IsAttacking, true);
    }

    public override void UpdateState()
    {
        HandleCombo();
        CheckSwitchStates();
        timer -= Time.deltaTime;
    }

    public override void ExitState()
    {
        timer = 0;
        Context.Animator.SetBool(Context.IsAttacking, false);
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

        if (Context.IsMousePressed)
        {
            Context.RequireNewAttack = true;
        }
        //Context.CurrentAttackResetRoutine = Context.StartCoroutine(IAttackResetRoutine());
        if (Context.AttackCount == 4)
        {
            Context.AttackCount = 0;
            Context.Animator.SetInteger(Context.AttackIndexHash, Context.AttackCount);
        }
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsMovementPressed && timer <= 0)
        {
            SwitchState(Factory.Idle());
        }
        if (Context.IsMovementPressed && timer <= 0)
        {
            SwitchState(Factory.Walk());
        }
        if (Context.IsJumpPressed && !Context.RequireNewJump && timer <= 0)
        {
            SwitchState(Factory.Jump());
        }
    }

    void HandleCombo()
    {

        if (Context.Animator.IsInTransition(3) || Context.Animator.GetCurrentAnimatorStateInfo(3).IsName("Ellen_Combo4")) return;
      
        if (Context.WasMouseLeftPressedThisFrame && Context.Animator.GetCurrentAnimatorStateInfo(3).normalizedTime >= 0.3f)
        {
            attackIndex = Context.Animator.GetInteger(Context.AttackComboIndexHash);
            timer = 0.8f;
            Context.Animator.SetBool(Context.PlayerComboInputHash,true);
            currentTime = Time.time;
        }
        





        //Context.Animator.SetBool(Context.IsAttacking, true);
        //currentTime = Time.time;
        //timer = 0.4f; //0.55f
        //if (Time.time > currentTime + 0.2f)
        //{
        //    Context.Animator.SetBool(Context.IsAttacking, false);
        //}

        //if (Context.AttackCount < 4 && Context.CurrentAttackResetRoutine != null)
        //{
        //    Context.StopCoroutine(Context.CurrentAttackResetRoutine);
        //}

        //Context.AttackCount += 1;
        //Context.IsAttack = true;
        //Context.Animator.SetInteger(Context.AttackIndexHash, Context.AttackCount);
    }
    public override void InitializeSubState()
    {
    }


}