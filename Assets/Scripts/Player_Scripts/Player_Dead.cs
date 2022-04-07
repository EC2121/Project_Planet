using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dead : Player_BaseState
{
    public Player_Dead(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
      //  InitializeSubState();
    }

    public override void EnterState()
    {
        Context.Animator.SetBool("isDead", true);
        
        Context.Animator.SetBool(Context.IsWalkingHash,false);
        Context.Animator.SetBool(Context.IsRunningHash,false);
       
       
        //Context.AppliedMovementY = 0;
    }

    public override void UpdateState()
    {
        Context.CurrentMovementX = 0;
        Context.CurrentMovementZ = 0;
       // Context.AppliedMovementX = 0;
        //Context.AppliedMovementZ = 0;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.Log("TEESTTTTTTTTTTTTT");
    }

    public override void CheckSwitchStates()
    {
        if (Context.Hp > 0)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
    }
}