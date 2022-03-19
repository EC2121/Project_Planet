using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_Idle : Script_AI_Roby_BaseState
{
    private float time = 2;

    public override void OnEnter(Script_AI_Roby_MGR AIRoby)
    {
        AIRoby.Owner.StopRoby();
    }

    public override void OnExit(Script_AI_Roby_MGR AIRoby)
    {
        time = 2;
    }

    public override void OnTriggerEnter(Script_AI_Roby_MGR AiRoby, Collider collider)
    {
        AiRoby.Owner.EnemysInArea(collider.gameObject);
        AiRoby.SwitchState(AiRoby.AI_Roby_BattleState);
    }

    public override void UpdateState(Script_AI_Roby_MGR AIRoby)
    {
        if (AIRoby.Owner.IsMaITooFar()) AIRoby.SwitchState(AIRoby.AiRobyFollowState);


        time -= Time.deltaTime;
        if (time <= 0)
        {
            float activity = Random.Range(0, 2);
            switch (activity)
            {
                case 0:
                    AIRoby.SwitchState(AIRoby.AiRobyPatroll);
                    break;
                case 1:
                    time = 3;
                    break;
            }
        }
    }
}