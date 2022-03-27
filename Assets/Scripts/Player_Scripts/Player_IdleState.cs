using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Player_IdleState :  Player_BaseState
{
    public Player_IdleState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(currentContext,playerStateFactory)
    {
        
    }
    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsWalkingHash,false);
        Context.Animator.SetBool(Context.IsRunningHash,false);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
       
    }

    public override void CheckSwitchStates()
    {
        if (Context.IsMovementPressed && Context.IsRunPressed)
        {
            SwitchState(Factory.Run());
        }
        else if (Context.IsMovementPressed)
        {
            SwitchState(Factory.Walk());
        }
    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }
}
