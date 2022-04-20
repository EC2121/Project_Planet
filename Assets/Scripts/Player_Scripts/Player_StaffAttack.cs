using UnityEngine;

public class Player_StaffAttack : Player_BaseState
{
    private float timer = 0.565f;

    public Player_StaffAttack(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {

    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
        HandleCombo();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsAttacking, false);

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
        if (Context.IsMovementPressed && Context.IsRunPressed && timer <= 0)
        {
            SwitchState(Factory.Run());
        }

        if (Context.IsIsHitted)
        {
            SwitchState(Factory.Hitted());
        }
    }

    private void HandleCombo()
    {
        Context.Animator.SetBool(Context.IsAttacking, true);

        if (Context.Animator.IsInTransition(0) || Context.Animator.GetCurrentAnimatorStateInfo(0).IsName("Ellen_Combo4")) return;

        if (Context.IsMousePressed && Context.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
        {
            timer = 0.8f;
            Context.Animator.SetBool("ComboInput", true);
        }
    }
    public override void InitializeSubState()
    {
    }
}