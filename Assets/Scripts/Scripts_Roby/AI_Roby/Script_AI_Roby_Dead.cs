using UnityEngine;

public class Script_AI_Roby_Dead : Script_AI_Roby_BaseState
{
    public void CustomCollisionEnter(Script_Roby AiRoby, Collision other) { }

    public void CustomOnTriggerEnter(Script_Roby AiRoby, Collider collider) { }

    public void CustomOnTriggerExit(Script_Roby AiRoby, Collider collider) { }

    public void CustomOnTriggerStay(Script_Roby AiRoby, Collider collider) { }
    public void OnExit(Script_Roby AIRoby) { }

    public void UpdateState(Script_Roby AIRoby) { }

    public void OnEnter(Script_Roby AIRoby)
    {
        AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_Dead);
    }
}
