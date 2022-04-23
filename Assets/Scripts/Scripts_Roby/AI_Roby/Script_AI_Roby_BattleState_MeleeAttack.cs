using UnityEngine;

public class Script_AI_Roby_BattleState_MeleeAttack : Script_AI_Roby_BaseState
{
    public void CustomCollisionEnter(Script_Roby AiRoby, Collision other)
    {
        Player_State_Machine.hit.Invoke();
    }

    public void CustomOnTriggerEnter(Script_Roby AiRoby, Collider collider) { }

    public void CustomOnTriggerExit(Script_Roby AiRoby, Collider collider)
    {
        AiRoby.EnemyOutArea(collider.gameObject);
    }

    public void CustomOnTriggerStay(Script_Roby AiRoby, Collider collider) { }

    public void OnEnter(Script_Roby AIRoby)
    {
        AIRoby.SetPath(AIRoby.Roby_EnemyTarget.transform.position);
        AIRoby.Roby_Animator.SetBool(AIRoby.Roby_AshAnimator_walk, true);
        AIRoby.Roby_Animator.SetFloat(AIRoby.Roby_AshAnimator_walkSpeed, 0);

        if (AIRoby.roby_EnemysInMyArea.Count == 0 || ReferenceEquals(AIRoby.Roby_EnemyTarget, null))
            AIRoby.SwitchState(RobyStates.Idle);
    }

    public void OnExit(Script_Roby AIRoby)
    {
        AIRoby.Roby_NavAgent.ResetPath();
    }

    public void UpdateState(Script_Roby AIRoby)
    {
        if (AIRoby.roby_EnemysInMyArea.Count <= 0 || ReferenceEquals(AIRoby.Roby_EnemyTarget, null))
        {
            AIRoby.SwitchState(RobyStates.Idle);
            return;
        }
        AIRoby.printme(AIRoby.roby_EnemysInMyArea.Count.ToString());
        AIRoby.printme(ReferenceEquals(AIRoby.Roby_EnemyTarget, null).ToString());

        if (AIRoby.Roby_NavAgent.remainingDistance < AIRoby.Roby_NavAgent.stoppingDistance && AIRoby.roby_EnemysInMyArea.Count > 1 || !ReferenceEquals(AIRoby.Roby_EnemyTarget, null))
        {
            AIRoby.Roby_Animator.SetBool(AIRoby.Roby_AshAnimator_walk, false);
            TurnTowardsEnemy(AIRoby);

            AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_Melee);
            AIRoby.SwitchState(RobyStates.Battle);
        }
    }

    public void TurnTowardsEnemy(Script_Roby AIRoby)
    {
        float angle = AngleCalculator(AIRoby);

        if (angle > -10 && angle < 10) return;

        angle = AIRoby.InverseClamp(-10, 10, angle);
        angle /= 180;
        AIRoby.Roby_Animator.SetFloat("Angle", angle);

        AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_turnTrigger);
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
}
