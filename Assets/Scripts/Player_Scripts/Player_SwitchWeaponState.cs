using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SwitchWeaponState : Player_BaseState
{
    public Player_SwitchWeaponState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;

        InitializeSubState();
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        ActivateWeapon();
    }

    public override void ExitState()
    {
        if (Context.IsSwitchPressed)
        {
            Context.RequireNewWeaponSwitch = true;
        }
    }

    public override void CheckSwitchStates()
    {
        if (!Context.RequireNewWeaponSwitch)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
    }

    public void ActivateWeapon()
    {
        if (!Context.IsWeaponAttached && !Context.IsMovementPressed)
        {
            Context.IsWeaponAttached = true;
            Context.IsSwitchPressed = false;
            Context.Sockets.Attach(Context.Weapon, Handle_Mesh_Sockets.SocketId.Spine);
            Context.Animator.SetBool(Context.EquipHash, true);
        }
        else if (Context.IsWeaponAttached && !Context.IsMovementPressed)
        {
            Context.IsWeaponAttached = false;
            Context.IsSwitchPressed = false;

            Context.Sockets.Attach(Context.Weapon, Handle_Mesh_Sockets.SocketId.RightHand);
            Context.Animator.SetBool(Context.EquipHash, false);
        }
    }

    // public void OnAnimationEvent(string eventName)
    // {
    //     if (eventName == "EquipWeapon")
    //     {
    //         Context.Sockets.Attach(Context.Weapon, Handle_Mesh_Sockets.SocketId.RightHand);
    //     }
    //
    //     if (eventName == "Detach")
    //     {
    //         Context.Sockets.Attach(Context.Weapon, Handle_Mesh_Sockets.SocketId.Spine);
    //     }
    // }
}