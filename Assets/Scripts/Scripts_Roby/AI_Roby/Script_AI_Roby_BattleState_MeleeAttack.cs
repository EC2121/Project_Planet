using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_BattleState_MeleeAttack : Script_AI_Roby_BaseState
{
    public void CustomCollisionEnter(Script_Roby AiRoby, Collision other)
    {

        AiRoby.transform.LookAt(AiRoby.Roby_EnemyTarget.transform);
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().AddDamage(50,AiRoby.gameObject,false);
        }
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
        //float angle = AngleCalculator(AIRoby);
        ////AIRoby.PrintMe(angle.ToString());

        //if (angle > -10 && angle < 10) return;

        //angle = AIRoby.InverseClamp(-10, 10, angle);
        //angle /= 180;
        //AIRoby.Roby_Animator.SetFloat("Angle", angle);

        //AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_turnTrigger);

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

    public float AngleCalculator(Script_Roby AIRoby)
    {
        Vector3 MyForw = AIRoby.transform.forward;
        float dot = Vector3.Dot(MyForw, (AIRoby.Roby_EnemyTarget.transform.position - AIRoby.transform.position).normalized);
        Vector3 Cross = Vector3.Cross(MyForw, (AIRoby.Roby_EnemyTarget.transform.position - AIRoby.transform.position).normalized);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (Mathf.Sign(Cross.y) == -1)
            return -angle;
        else
            return angle;
    }
}
