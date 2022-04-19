using System.Collections;
using UnityEngine;

public class Script_AI_Roby_BreakWall : Script_AI_Roby_BaseState
{
    private Transform ForceOriginTransform;
    private bool needsRot;
    private float timer;
    public void OnEnter(Script_Roby AIRoby)
    {
        needsRot = true;
        AIRoby.Roby_Animator.SetBool(AIRoby.Roby_AshAnimator_walk, true);
        AIRoby.Roby_Animator.SetFloat(AIRoby.Roby_AshAnimator_walkSpeed, 1);
        AIRoby.Roby_NavAgent.updatePosition = true;
        AIRoby.Roby_Animator.applyRootMotion = false;
        ForceOriginTransform = AIRoby.wallToBreak.GetComponentInParent<BreakableObject>().ForceOrigin;
        AIRoby.SetPath(ForceOriginTransform.position);
        timer = 0;
    }

    public void OnExit(Script_Roby AIRoby)
    {
        AIRoby.Roby_IgnoreEnemy = false;
        AIRoby.Roby_Animator.SetBool(AIRoby.Roby_AshAnimator_walk, false);
        AIRoby.Roby_NavAgent.ResetPath();
        AIRoby.Roby_NavAgent.updatePosition = false;
        AIRoby.Roby_Animator.applyRootMotion = true;
        AIRoby.wallToBreak = null;
    }

    public void CustomOnTriggerEnter(Script_Roby AiRoby, Collider collider)
    {
        
    }

    public void CustomOnTriggerExit(Script_Roby AiRoby, Collider collider)
    {
    }

    public void CustomOnTriggerStay(Script_Roby AiRoby, Collider collider)
    {
        
    }

    public void UpdateState(Script_Roby AIRoby)
    {

        if (needsRot && AIRoby.Roby_NavAgent.remainingDistance < AIRoby.Roby_NavAgent.stoppingDistance)
        {
            AIRoby.Roby_Animator.SetBool(AIRoby.Roby_AshAnimator_walk, false);
            float roby_AttackAngle = AngleCalculator(AIRoby);
            needsRot = false;
            if (roby_AttackAngle < -10 || roby_AttackAngle > 10)
            {
                roby_AttackAngle /= 180;
                AIRoby.Roby_Animator.SetFloat("Angle", roby_AttackAngle);
                AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_turnTrigger);
            }
                AIRoby.StartCoroutine(BreakWallCoroutine(AIRoby));
        }
        timer += Time.deltaTime;
        if (timer >= 30f)
        {
            AIRoby.SwitchState(RobyStates.Idle);
        }
    }

    IEnumerator BreakWallCoroutine(Script_Roby AIRoby)
    {
        AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_Melee);
        yield return new WaitForSeconds(2f);
        AIRoby.SwitchState(RobyStates.Idle);
    }

    public void CustomCollisionEnter(Script_Roby AiRoby, Collision other) { }
    public float AngleCalculator(Script_Roby AIRoby)
    {
        Vector3 MyForw = AIRoby.transform.forward;
        float dot = Vector3.Dot(MyForw, (ForceOriginTransform.forward));
        Vector3 Cross = Vector3.Cross(MyForw, (ForceOriginTransform.position - AIRoby.transform.position).normalized);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (Mathf.Sign(Cross.y) == -1)
            return -angle;
        else
            return angle;
    }
}
