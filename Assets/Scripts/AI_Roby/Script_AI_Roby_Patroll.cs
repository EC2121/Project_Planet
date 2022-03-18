using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_Patroll : Script_AI_Roby_BaseState
{
    private int walkSpeedAsh;
    private int walkAsh;

    private float mai_Distance;
    private float mai_NearZone;

    private Vector3 mai_Position;

    private void Start()
    {
        walkSpeedAsh = Animator.StringToHash("Speed");
        walkAsh = Animator.StringToHash("InPursuit");
    }

    public override void OnEnter(Script_AI_Roby_MGR AIRoby)
    {
        AIRoby.RobyMGR.Roby_animator.SetFloat(walkSpeedAsh, 0);
        AIRoby.RobyMGR.Roby_animator.SetBool(walkAsh, true);

        mai_Position = AIRoby.RobyMGR.Mai_Player.transform.position;
        mai_Distance = AIRoby.RobyMGR.Mai_PlayerDistanceZone;
        mai_NearZone = AIRoby.RobyMGR.Mai_PlayerNearZone;

        AIRoby.RobyMGR.Roby_agent.SetDestination
            (mai_Position + new Vector3(InverseClamp(mai_Position.x - mai_Distance, mai_Position.x + mai_Distance, Random.insideUnitCircle.x * mai_NearZone),
            0,
            InverseClamp(mai_Position.z - mai_Distance, mai_Position.z + mai_Distance, Random.insideUnitCircle.y * mai_NearZone)));
    }

    public override void UpdateState(Script_AI_Roby_MGR AIRoby)
    {
        if (AIRoby.RobyMGR.Roby_agent.remainingDistance < AIRoby.RobyMGR.Roby_agent.stoppingDistance) AIRoby.SwitchState(AIRoby.AIRobyIdle);
    }

    public override void OnExit(Script_AI_Roby_MGR AIRoby)
    {
        AIRoby.RobyMGR.Roby_animator.SetBool(walkAsh, false);
        AIRoby.RobyMGR.Roby_agent.ResetPath();
    }

    private float InverseClamp(float min, float max, float value)
    {
        if (value > min && value < max)
        {
            float value_min = Mathf.Abs(min - value);
            float value_max = Mathf.Abs(max - value);
            float result = Mathf.Min(value_max, value_min);
            if (result == value_max) return max;
            else return min;
        }
        return value;
    }
}
