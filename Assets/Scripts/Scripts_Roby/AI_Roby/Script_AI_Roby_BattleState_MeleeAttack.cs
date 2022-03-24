using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_BattleState_MeleeAttack : Script_AI_Roby_BaseState
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
        AIRoby.SetPath(AIRoby.Roby_EnemyTarget.transform.position);
        AIRoby.Roby_Animator.SetBool(AIRoby.Roby_AshAnimator_walk, true);
        //AIRoby.Roby_NavAgent.SetDestination(AIRoby.Roby_EnemyTarget.transform.position);
    }

    public void OnExit(Script_Roby AIRoby)
    {
        AIRoby.Roby_NavAgent.ResetPath();
        //AIRoby.Roby_Animator.ResetTrigger(AIRoby.roby_Animator_MeleeAsh);
    }

    public void UpdateState(Script_Roby AIRoby)
    {
        if (AIRoby.roby_EnemysInMyArea.Count == 0)
        {
            AIRoby.SwitchState(RobyStates.Idle);
            return;
        }

        if (AIRoby.Roby_NavAgent.remainingDistance < AIRoby.Roby_NavAgent.stoppingDistance)
        {
            AIRoby.Roby_Animator.SetBool(AIRoby.Roby_AshAnimator_walk, false);

            AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_Melee);
            AIRoby.SwitchState(RobyStates.Battle);
        }
    }
}
