using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Ai_Roby_BattleState_ZoneAttack : Script_AI_Roby_BaseState
{
    public void CustomCollisionEnter(Script_Roby AiRoby, Collision other)
    {
    }

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
        AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_Zone);
    }

    public void OnExit(Script_Roby AIRoby)
    {
    }

    public void UpdateState(Script_Roby AIRoby)
    {
    }
}
