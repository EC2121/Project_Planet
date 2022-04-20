using UnityEngine;

public class Player_SwitchWeaponState : Player_BaseState
{
    private float timer = 2f;
    private bool canExit = false;

    public Player_SwitchWeaponState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
       
    }

    public override void EnterState()
    {
        ActivateWeapon();
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
       
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        canExit = false;
        if (!Context.IsRunPressed)
        {
            Context.Animator.SetBool(Context.IsRunningHash, false);

            Context.AppliedMovementX = Context.CurrentMovementInput.x;
            Context.AppliedMovementZ = Context.CurrentMovementInput.y;
        }

        if (!Context.IsMovementPressed)
        {
            Context.Animator.SetBool(Context.IsRunningHash, false);
            Context.Animator.SetBool(Context.IsWalkingHash, false);
            Context.AppliedMovementX = 0;
            Context.AppliedMovementZ = 0;
        }
        if (Context.IsSwitchPressed)
        {
            Context.RequireNewWeaponSwitch = true;
        }
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsMovementPressed && !Context.IsRunPressed && Context.Animator.IsInTransition(1))
        {
            SwitchState(Factory.Idle());
        }

        if (Context.IsMovementPressed && !Context.IsRunPressed && Context.Animator.IsInTransition(1))
        {
            SwitchState(Factory.Walk());
        }

        if (Context.IsRunPressed && !Context.HasBox && Context.Animator.IsInTransition(1))
        {
            SwitchState(Factory.Run());
        }
    }

    public override void InitializeSubState()
    {
    }

    public void ActivateWeapon()
    {
        if (!Context.IsWeaponAttached)
        {
            Context.IsWeaponAttached = true;
            canExit = true;
            Context.Sockets.Attach(Context.Weapon, Handle_Mesh_Sockets.SocketId.Spine);
            Context.Animator.SetBool(Context.EquipHash, true);
        }
        else if (Context.IsWeaponAttached)
        {
            Context.IsWeaponAttached = false;
            canExit = true;
            Context.Sockets.Attach(Context.Weapon, Handle_Mesh_Sockets.SocketId.RightHand);
            Context.Animator.SetBool(Context.EquipHash, false);
        }
    }
}