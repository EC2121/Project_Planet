using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_FollowState : Script_AI_Roby_BaseState
{
    public override void OnEnter(Script_AI_Roby_MGR AIRoby)
    {
        AIRoby.Owner.roby_Animator.SetBool("InPursuit", true);
    }

    public override void OnExit(Script_AI_Roby_MGR AIRoby)
    {
    }

    public override void OnTriggerEnter(Script_AI_Roby_MGR AiRoby, Collider collider)
    {
        AiRoby.Owner.EnemysInArea(collider.gameObject);
        AiRoby.SwitchState(AiRoby.AI_Roby_BattleState);
    }

    public override void UpdateState(Script_AI_Roby_MGR AIRoby)
    {
        if (AIRoby.Owner.CheckRemainingDistance()) AIRoby.SwitchState(AIRoby.AIRobyIdle);

        AIRoby.Owner.FollowPlayer();
    }

}
