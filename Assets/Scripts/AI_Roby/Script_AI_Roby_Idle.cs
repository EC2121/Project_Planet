using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_Idle : Script_AI_Roby_BaseState
{
    private float time = 2;

    public override void OnEnter(Script_AI_Roby_MGR AIRoby)
    {
    }

    public override void OnExit(Script_AI_Roby_MGR AIRoby)
    {
        time = 2;
    }

    public override void UpdateState(Script_AI_Roby_MGR AIRoby)
    {
        //if (Vector3.Distance(AIRoby.RobyMGR.transform.position, AIRoby.RobyMGR.Mai_Player.transform.position) > AIRoby.RobyMGR.Mai_PlayerNormalZone) { }

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
