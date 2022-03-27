using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_Patroll : Script_AI_Roby_BaseState
{
    private Vector3 roby_PatrollingPoint;
    public void OnEnter(Script_Roby AIRoby)
    {
        AIRoby.Roby_Animator.SetFloat(AIRoby.Roby_AshAnimator_walkSpeed, 0);
        AIRoby.Roby_Animator.SetBool(AIRoby.Roby_AshAnimator_walk, true);

        roby_PatrollingPoint = AIRoby.Mai_Player.transform.position +
            new Vector3(AIRoby.InverseClamp(AIRoby.Mai_Player.transform.position.x - AIRoby.Mai_MinDistance, AIRoby.Mai_Player.transform.position.x + AIRoby.Mai_MinDistance, Random.insideUnitCircle.x * AIRoby.Mai_PlayerNearZone), 0,
            AIRoby.InverseClamp(AIRoby.Mai_Player.transform.position.z - AIRoby.Mai_MinDistance, AIRoby.Mai_Player.transform.position.z + AIRoby.Mai_MinDistance, Random.insideUnitCircle.y * AIRoby.Mai_PlayerNearZone));
        
        AIRoby.SetPath(roby_PatrollingPoint);
    }

    public void OnExit(Script_Roby AIRoby)
    {
        AIRoby.Roby_Animator.SetBool(AIRoby.Roby_AshAnimator_walk, false);
        AIRoby.Roby_NavAgent.ResetPath();
    }

    public void CustomOnTriggerEnter(Script_Roby AiRoby, Collider collider)
    {
        if (AiRoby.EnemysInArea(collider.gameObject))
            AiRoby.SwitchState(RobyStates.Battle);
    }

    public void CustomOnTriggerStay(Script_Roby AiRoby, Collider collider)
    {
        //if (AiRoby.EnemysInArea(collider.gameObject))
        //    AiRoby.SwitchState(RobyStates.Battle);
    }

    public void CustomOnTriggerExit(Script_Roby AiRoby, Collider collider)
    {
        AiRoby.EnemyOutArea(collider.gameObject);
    }
    public void UpdateState(Script_Roby AIRoby)
    {
        if (AIRoby.IsMaITooFar(AIRoby.Mai_PlayerNormalZone))
        {
            AIRoby.SwitchState(RobyStates.Follow);
            return;
        }

        if (AIRoby.Roby_NavAgent.remainingDistance < AIRoby.Roby_NavAgent.stoppingDistance) AIRoby.SwitchState(RobyStates.Idle);
    }
}
