using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_Dead : Script_AI_Roby_BaseState
{
    public override void CustomOnTriggerEnter(Script_AI_Roby_MGR AiRoby, Collider collider)
    {
    }

    public override void CustomOnTriggerExit(Script_AI_Roby_MGR AiRoby, Collider collider)
    {
    }

    public override void CustomOnTriggerStay(Script_AI_Roby_MGR AiRoby, Collider collider)
    {
    }

    public override void OnEnter(Script_AI_Roby_MGR AIRoby)
    {
        AIRoby.Owner.RobyDie();
    }

    public override void OnExit(Script_AI_Roby_MGR AIRoby)
    {
    }

    public override void UpdateState(Script_AI_Roby_MGR AIRoby)
    {
    }
}
