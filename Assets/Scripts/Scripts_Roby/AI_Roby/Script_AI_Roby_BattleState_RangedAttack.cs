using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_BattleState_RangedAttack : Script_AI_Roby_BaseState
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
        AIRoby.PrintMe("rangeAttack");
        AIRoby.Roby_Animator.SetTrigger(AIRoby.roby_Animator_RangeAsh);
        AIRoby.SetPath(AIRoby.Roby_EnemyTarget.transform.position);
    }

    public void OnExit(Script_Roby AIRoby)
    {
        AIRoby.Roby_Animator.ResetTrigger(AIRoby.roby_Animator_RangeAsh);
    }

    public void UpdateState(Script_Roby AIRoby)
    {
        if (ReferenceEquals(AIRoby.Roby_EnemyTarget, null)) return;

            if (AIRoby.roby_EnemysInMyArea.Count == 0)
            {
                AIRoby.SwitchState(RobyStates.Idle);
                return;
            }
        AIRoby.roby_Particle_Shoot.transform.LookAt(AIRoby.Roby_EnemyTarget.transform.position);
        //float angle = AngleCalculator(AIRoby);
        //if (angle <= 180) AIRoby.Roby_Animator.SetFloat("Angle", angle / 180);
        //else AIRoby.Roby_Animator.SetFloat("Angle", -(angle / 360));
        //AIRoby.Roby_Animator.SetTrigger(AIRoby.animator_turnTriggerAsh);
    }

    public float AngleCalculator(Script_Roby AIRoby)
    {
        Vector3 MyForw = AIRoby.transform.forward;
        float dot = Vector3.Dot(MyForw, (AIRoby.Roby_EnemyTarget.transform.position - AIRoby.transform.position).normalized);
        Vector3 Cross = Vector3.Cross(MyForw, (AIRoby.Roby_EnemyTarget.transform.position - AIRoby.transform.position).normalized);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (Mathf.Sign(Cross.y) == -1)
            return 360 - angle;
        else
            return angle;
    }
}
