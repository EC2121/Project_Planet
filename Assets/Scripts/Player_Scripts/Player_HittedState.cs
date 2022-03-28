using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HittedState : Player_BaseState
{
    public Player_HittedState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
       Debug.Log("OH SI");
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
          SwitchState(Factory.Grounded());
    }

    public override void InitializeSubState()
    {
      
    }
}
