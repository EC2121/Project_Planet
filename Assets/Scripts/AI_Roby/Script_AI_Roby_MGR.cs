using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_MGR : MonoBehaviour
{
    [HideInInspector] public Script_Roby_MGR RobyMGR;
    [HideInInspector] public Script_AI_Roby_Idle AIRobyIdle;
    [HideInInspector] public Script_AI_Roby_Patroll AiRobyPatroll;

    Script_AI_Roby_BaseState currentState;

    private void Awake()
    {
        AIRobyIdle = new Script_AI_Roby_Idle();
        AiRobyPatroll = new Script_AI_Roby_Patroll();
        RobyMGR = new Script_Roby_MGR();
        currentState = AIRobyIdle;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(Script_AI_Roby_BaseState state)
    {
        currentState.OnExit(this);
        currentState = state;
        currentState.OnEnter(this);
    }
}
