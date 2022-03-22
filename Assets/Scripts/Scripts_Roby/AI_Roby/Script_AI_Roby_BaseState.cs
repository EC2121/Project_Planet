using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Script_AI_Roby_BaseState : MonoBehaviour
{
    public abstract void OnEnter(Script_AI_Roby_MGR AIRoby);
    public abstract void OnExit(Script_AI_Roby_MGR AIRoby);
    public abstract void CustomOnTriggerEnter(Script_AI_Roby_MGR AiRoby, Collider collider);
    public abstract void CustomOnTriggerStay(Script_AI_Roby_MGR AiRoby, Collider collider);
    public abstract void CustomOnTriggerExit(Script_AI_Roby_MGR AiRoby, Collider collider);
    public abstract void UpdateState(Script_AI_Roby_MGR AIRoby);
}
