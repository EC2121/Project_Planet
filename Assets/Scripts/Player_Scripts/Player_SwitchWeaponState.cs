using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SwitchWeaponState : Player_BaseState
{
    private float timer = 1.2f;

    public Player_SwitchWeaponState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        // IsRootState = true;

        //  InitializeSubState();
    }

    public override void EnterState()
    {
        ActivateWeapon();
        Context.Animator.SetBool(Context.IsWalkingHash, false);
        Context.Animator.SetBool(Context.IsRunningHash, false);
        Context.AppliedMovementX = 0;
        Context.AppliedMovementZ = 0;
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
        CheckSwitchStates();
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
       
        if (!Context.IsMovementPressed && !Context.IsRunPressed && timer <= 0)
        {
            SwitchState(Factory.Idle());
        }
        else if (Context.IsMovementPressed && !Context.IsRunPressed && timer <= 0)
        {
            SwitchState(Factory.Walk());
        }
        else if (Context.IsRunPressed && timer <= 0)
        {
            SwitchState(Factory.Run());
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
            Context.Sockets.Attach(Context.Weapon, Handle_Mesh_Sockets.SocketId.Spine);
            Context.Animator.SetBool(Context.EquipHash, true);
        }
        else if (Context.IsWeaponAttached && !Context.IsMovementPressed)
        {
            Context.IsWeaponAttached = false;

            Context.Sockets.Attach(Context.Weapon, Handle_Mesh_Sockets.SocketId.RightHand);
            Context.Animator.SetBool(Context.EquipHash, false);
        }
    }
}