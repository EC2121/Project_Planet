using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Script_AI_Roby_BaseState : MonoBehaviour
{
    public abstract void OnEnter(Script_AI_Roby_MGR AIRoby);
    public abstract void OnExit(Script_AI_Roby_MGR AIRoby);
    public abstract void OnTriggerEnter(Script_AI_Roby_MGR AiRoby, Collider collider);
    public abstract void OnTriggerSaty(Script_AI_Roby_MGR AiRoby, Collider collider);
    public abstract void OnTriggerExit(Script_AI_Roby_MGR AiRoby, Collider collider);
    public abstract void UpdateState(Script_AI_Roby_MGR AIRoby);
}
