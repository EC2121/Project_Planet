using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Script_AI_Roby_FollowState : Script_AI_Roby_BaseState
{
    private Vector3 roby_NearestpointOnEdge;
    public void OnEnter(Script_Roby AIRoby)
    {
        roby_NearestpointOnEdge = AIRoby.Mai_Player.transform.position + (AIRoby.Mai_PlayerNearZone) * (Vector3.Normalize(AIRoby.transform.position - AIRoby.Mai_Player.transform.position));
        AIRoby.Roby_Animator.SetBool(AIRoby.animator_walkAsh, true);
        AIRoby.Roby_Animator.SetFloat(AIRoby.animator_walkSpeedAsh, 1);
        AIRoby.SetPath(roby_NearestpointOnEdge);
    }

    public void OnExit(Script_Roby AIRoby)
    {
        AIRoby.Roby_IgnoreEnemy = false;
        AIRoby.Roby_Animator.SetBool(AIRoby.animator_walkAsh, false);
        AIRoby.Roby_Animator.SetFloat(AIRoby.animator_walkSpeedAsh, 0);
        AIRoby.Roby_NavAgent.ResetPath();
    }

    public void CustomOnTriggerEnter(Script_Roby AiRoby, Collider collider)
    {
        if (!AiRoby.Roby_IgnoreEnemy && AiRoby.EnemysInArea(collider.gameObject))
        {
            AiRoby.SwitchState(RobyStates.Battle);
            return;
        }
    }

    public void CustomOnTriggerExit(Script_Roby AiRoby, Collider collider)
    {
        AiRoby.EnemyOutArea(collider.gameObject);
    }

    public void CustomOnTriggerStay(Script_Roby AiRoby, Collider collider)
    {
        if (!AiRoby.Roby_IgnoreEnemy && AiRoby.EnemysInArea(collider.gameObject))
        {
            AiRoby.SwitchState(RobyStates.Battle);
            return;
        }
    }

    public void UpdateState(Script_Roby AIRoby)
    {
        if (AIRoby.Roby_NavAgent.remainingDistance < AIRoby.Roby_NavAgent.stoppingDistance) AIRoby.SwitchState(RobyStates.Idle);
    }

}
