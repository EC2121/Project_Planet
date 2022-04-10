using UnityEngine;

public class Script_AI_Roby_BattleState_RangedAttack : Script_AI_Roby_BaseState
{
    public void CustomOnTriggerEnter(Script_Roby AiRoby, Collider collider)
    {
        AiRoby.EnemysInArea(collider.gameObject);
    }

    public void CustomOnTriggerExit(Script_Roby AiRoby, Collider collider)
    {
        AiRoby.EnemyOutArea(collider.gameObject);
    }

    public void CustomOnTriggerStay(Script_Roby AiRoby, Collider collider) { }

    public void OnEnter(Script_Roby AIRoby)
    {
        float roby_AttackAngle = AngleCalculator(AIRoby);

        //if (angle > -10 && angle < 10)
        //{
        //    return;
        //}
        if (roby_AttackAngle < -10 || roby_AttackAngle > 10)
        {
            roby_AttackAngle /= 180;
            AIRoby.Roby_Animator.SetFloat("Angle", roby_AttackAngle);
            AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_turnTrigger);
        }
        //else if (roby_AttackAngle > -10 && roby_AttackAngle < 10)
        //{
        //    //AIRoby.Roby_Particle_Shoot.transform.LookAt(AIRoby.Roby_EnemyTarget.transform.position + new Vector3(0, AIRoby.Roby_EnemyTarget.transform.localScale.y * 0.5f, 0));
        //}
        //roby_AttackAngle = AIRoby.InverseClamp(-10, 10, roby_AttackAngle);
        AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_Range);
    }

    public void OnExit(Script_Roby AIRoby)
    {
        AIRoby.Roby_Animator.ResetTrigger(AIRoby.Roby_AshAnimator_Range);
        AIRoby.Roby_Animator.SetBool(AIRoby.Roby_AshAnimator_walk, false);
    }

    public void UpdateState(Script_Roby AIRoby)
    {
        if (ReferenceEquals(AIRoby.Roby_EnemyTarget, null)) return;

        if (AIRoby.roby_EnemysInMyArea.Count == 0)
        {
            AIRoby.SwitchState(RobyStates.Idle);
        }
    }



    public float AngleCalculator(Script_Roby AIRoby)
    {
        Vector3 MyForw = AIRoby.transform.forward;
        float dot = Vector3.Dot(MyForw, ( AIRoby.Roby_EnemyTarget.transform.position - AIRoby.transform.position ).normalized);
        Vector3 Cross = Vector3.Cross(MyForw, ( AIRoby.Roby_EnemyTarget.transform.position - AIRoby.transform.position ).normalized);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (Mathf.Sign(Cross.y) == -1)
            return -angle;
        else
            return angle;
    }

    public void CustomCollisionEnter(Script_Roby AiRoby, Collision other)
    {
    }
}
