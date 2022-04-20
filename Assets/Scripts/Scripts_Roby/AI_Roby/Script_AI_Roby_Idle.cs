using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_Idle : Script_AI_Roby_BaseState
{
    private float time = 2;

    public  void OnEnter(Script_Roby AIRoby)
    {
        AIRoby.Roby_IgnoreEnemy = false;
    }

    public  void OnExit(Script_Roby AIRoby)
    {
        time = 2;
    }

    public  void CustomOnTriggerEnter(Script_Roby AiRoby, Collider collider)
    {
        if (AiRoby.EnemysInArea(collider.gameObject))
            AiRoby.SwitchState(RobyStates.Battle);
    }

    public  void CustomOnTriggerExit(Script_Roby AiRoby, Collider collider)
    {
        AiRoby.EnemyOutArea(collider.gameObject);
    }

    public void CustomOnTriggerStay(Script_Roby AiRoby, Collider collider)
    {
        if (AiRoby.EnemysInArea(collider.gameObject))
            AiRoby.SwitchState(RobyStates.Battle);
    }

    public  void UpdateState(Script_Roby AIRoby)
    {

        if (AIRoby.IsMaITooFar(AIRoby.Mai_PlayerNormalZone)) AIRoby.SwitchState(RobyStates.Follow);

        time -= Time.deltaTime;
        if (time <= 0)
        {
            float activity = Random.Range(0, 2);
            switch (activity)
            {
                case 0:
                    AIRoby.SwitchState(RobyStates.Patroll);
                    return;
                case 1:
                    time = 3;
                    break;
            }
        }
    }

    public void CustomCollisionEnter(Script_Roby AiRoby, Collision other)
    {
    }
}
