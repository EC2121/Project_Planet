using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SwitchWeaponState : Player_BaseState
{
    private float timer = 2f;
    private bool canExit = false;

    public Player_SwitchWeaponState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        // IsRootState = true;

        // InitializeSubState();
    }

    public override void EnterState()
    {
        ActivateWeapon();
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
        // Context.Animator.SetBool(Context.IsWalkingHash, false);
        // Context.Animator.SetBool(Context.IsRunningHash, false);
       // Context.AppliedMovementX = 0;
       // Context.AppliedMovementZ = 0;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        canExit = false;

        if (Context.IsSwitchPressed)
        {
            Context.RequireNewWeaponSwitch = true;
        }
    }

    public override void CheckSwitchStates()
    {
        // if (canExit)
        // {
        //     SwitchState(Factory.Grounded());
        // }
        if (!Context.IsMovementPressed && !Context.IsRunPressed && canExit)
        {
            SwitchState(Factory.Idle());
        }

        if (Context.IsMovementPressed && !Context.IsRunPressed && canExit)
        {
            SwitchState(Factory.Walk());
        }

        if (Context.IsRunPressed && !Context.HasBox && canExit)
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