using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Script_AI_Roby_BaseState
{
    public abstract void OnEnter(Script_Roby AIRoby);
    public abstract void OnExit(Script_Roby AIRoby);
    public abstract void CustomOnTriggerEnter(Script_Roby AiRoby, Collider collider);
    public abstract void CustomOnTriggerStay(Script_Roby AiRoby, Collider collider);
    public abstract void CustomOnTriggerExit(Script_Roby AiRoby, Collider collider);
    public abstract void UpdateState(Script_Roby AIRoby);
}
