using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_BattleState : Script_AI_Roby_BaseState
{
    public override void OnEnter(Script_AI_Roby_MGR AIRoby)
    {
    }

    public override void OnExit(Script_AI_Roby_MGR AIRoby)
    {
    }

    public override void OnTriggerEnter(Script_AI_Roby_MGR AiRoby, Collider collider)
    {
        
    }

    public override void UpdateState(Script_AI_Roby_MGR AIRoby)
    {
        print("Battle");
        if (AIRoby.Owner.EnemyWithinRange())
        {
            if (AIRoby.Owner.IsMaITooFar())
            {
                AIRoby.SwitchState(AIRoby.AiRobyFollowState);
            }
            else
            {
                AIRoby.Owner.Attack();
                AIRoby.SwitchState(AIRoby.AIRobyIdle);

            }
        }
        else
        {
            if (AIRoby.Owner.IsMaITooFar())
            {
                AIRoby.SwitchState(AIRoby.AiRobyFollowState);
            }
            else
            {
                AIRoby.SwitchState(AIRoby.AIRobyIdle);

            }
        }
    }

    
}
