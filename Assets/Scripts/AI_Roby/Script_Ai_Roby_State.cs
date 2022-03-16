using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract class Script_Ai_Roby_State : MonoBehaviour
{
    virtual public void OnEnter() { }
    virtual public void OnExit() { }

    protected StateMachine stateMachine;
    
    protected void SetStateMachine(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
