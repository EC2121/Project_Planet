using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_MGR : MonoBehaviour
{
    [HideInInspector] public Script_Roby Owner;
    [HideInInspector] public Script_AI_Roby_Idle AIRobyIdle;
    [HideInInspector] public Script_AI_Roby_Patroll AiRobyPatroll;
    [HideInInspector] public Script_AI_Roby_FollowState AiRobyFollowState;
    [HideInInspector] public Script_AI_Roby_BattleState AI_Roby_BattleState;
    [HideInInspector] public Script_AI_Roby_BaseState currentState;

    public Script_AI_Roby_MGR(Script_Roby owner)
    {

    }

    private void Awake()
    {
        AIRobyIdle = new Script_AI_Roby_Idle();
        AiRobyPatroll = new Script_AI_Roby_Patroll();
        AiRobyFollowState = new Script_AI_Roby_FollowState();
        AI_Roby_BattleState = new Script_AI_Roby_BattleState();
        currentState = AIRobyIdle;
    }
    private void Start()
    {
        Owner = GetComponent<Script_Roby>();
        currentState.OnEnter(this);
    }
    private void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(this, other);
    }

    public void SwitchState(Script_AI_Roby_BaseState state)
    {
        currentState.OnExit(this);
        currentState = state;
        currentState.OnEnter(this);
    }
}
