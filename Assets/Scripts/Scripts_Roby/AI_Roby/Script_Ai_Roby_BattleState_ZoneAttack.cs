using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Ai_Roby_BattleState_ZoneAttack : Script_AI_Roby_BaseState
{
    public void CustomOnTriggerEnter(Script_Roby AiRoby, Collider collider)
    {
    }

    public void CustomOnTriggerExit(Script_Roby AiRoby, Collider collider)
    {
        AiRoby.EnemyOutArea(collider.gameObject);
    }

    public void CustomOnTriggerStay(Script_Roby AiRoby, Collider collider)
    {
    }

    public void OnEnter(Script_Roby AIRoby)
    {
        AIRoby.Roby_Animator.SetTrigger(AIRoby.roby_Animator_ZoneAsh);
    }

    public void OnExit(Script_Roby AIRoby)
    {
    }

    public void UpdateState(Script_Roby AIRoby)
    {
    }
}
