using UnityEngine;

public class Script_AI_Roby_FollowState : Script_AI_Roby_BaseState
{
    private Vector3 roby_NearestpointOnEdge;
    
    
    public void OnEnter(Script_Roby AIRoby)
    {
        AIRoby.Roby_Animator.SetBool(AIRoby.Roby_AshAnimator_walk, true);
        AIRoby.Roby_Animator.SetFloat(AIRoby.Roby_AshAnimator_walkSpeed, 1);

        AIRoby.LeftFoot.Play();
        AIRoby.RightFoot.Play();

        AIRoby.Roby_NavAgent.updatePosition = true;
        AIRoby.Roby_Animator.applyRootMotion = false;
    }

    public void OnExit(Script_Roby AIRoby)
    {
        AIRoby.Roby_IgnoreEnemy = false;
        AIRoby.Roby_Animator.SetBool(AIRoby.Roby_AshAnimator_walk, false);
        AIRoby.LeftFoot.Stop();
        AIRoby.RightFoot.Stop();
        
        AIRoby.Roby_Animator.SetFloat(AIRoby.Roby_AshAnimator_walkSpeed, 0);
        AIRoby.Roby_NavAgent.ResetPath();
        AIRoby.Roby_NavAgent.updatePosition = false;
        AIRoby.Roby_Animator.applyRootMotion = true;
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
        if (AIRoby.Roby_Animator.GetCurrentAnimatorStateInfo(0).IsName(AIRoby.Roby_String_Animator_SkyWalkToStop)
            || AIRoby.Roby_Animator.IsInTransition(0))
            return;

        if (AIRoby.Mai_CharacterController.velocity != Vector3.zero)
        {
            roby_NearestpointOnEdge = AIRoby.Mai_Player.transform.position + ( AIRoby.Mai_PlayerNearZone ) * ( Vector3.Normalize(AIRoby.transform.position - AIRoby.Mai_Player.transform.position) );
            AIRoby.SetPath(roby_NearestpointOnEdge);
        }

        if (AIRoby.Roby_NavAgent.remainingDistance < AIRoby.Roby_NavAgent.stoppingDistance)
        {
            AIRoby.SwitchState(RobyStates.Idle);
            return;
        }
    }

    public void CustomCollisionEnter(Script_Roby AiRoby, Collision other) { }

}
