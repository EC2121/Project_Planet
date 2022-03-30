using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_BattleState : Script_AI_Roby_BaseState
{
    public void OnEnter(Script_Roby AIRoby)
    {
        if (AIRoby.roby_EnemysInMyArea.Count == 0) return;

        if (AIRoby.Roby_EnemyTarget == null || !AIRoby.Roby_EnemyTarget.activeInHierarchy)
        {
            AIRoby.Roby_Animator.SetFloat(AIRoby.Roby_AshAnimator_walkSpeed, 0);
            float lowest = float.MaxValue;
            for (int i = 0; i < AIRoby.roby_EnemysInMyArea.Count; i++)
            {
                if (ReferenceEquals(AIRoby.roby_EnemysInMyArea[i], null)) continue;

                float distanceFromEnemys = (Vector3.Distance(AIRoby.transform.position, AIRoby.roby_EnemysInMyArea[i].transform.position));
                if (distanceFromEnemys != 0 && distanceFromEnemys < lowest)
                {
                    lowest = distanceFromEnemys;
                    AIRoby.Roby_EnemyIndex = i;
                }
            }
            AIRoby.Roby_EnemyTarget = AIRoby.roby_EnemysInMyArea[AIRoby.Roby_EnemyIndex];
        }
    }

    public void OnExit(Script_Roby AIRoby)
    {
    }

    public void CustomOnTriggerEnter(Script_Roby AiRoby, Collider collider)
    {
        AiRoby.EnemysInArea(collider.gameObject);
    }

    public void CustomOnTriggerExit(Script_Roby AiRoby, Collider collider)
    {
        AiRoby.EnemyOutArea(collider.gameObject);
    }

    public void CustomOnTriggerStay(Script_Roby AiRoby, Collider collider)
    {
    }

    public void UpdateState(Script_Roby AIRoby)
    {
        if (AIRoby.roby_EnemysInMyArea.Count == 0 || ReferenceEquals(AIRoby.Roby_EnemyTarget, null))
        {
            AIRoby.SwitchState(RobyStates.Idle);
            return;
        }

        if (Vector3.Distance(AIRoby.transform.position, AIRoby.Roby_EnemyTarget.transform.position) < AIRoby.Roby_RobyNearZone)
        {
            if (AIRoby.IsMaITooFar(AIRoby.Mai_PlayerBattleZone))
            {
                if (AIRoby.Roby_Animator.GetCurrentAnimatorStateInfo(0).IsName("ZoneAttack")
                    || AIRoby.Roby_Animator.IsInTransition(0)) return;

                AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_Zone);
            }
            else
            {

                if (AIRoby.Roby_Animator.GetCurrentAnimatorStateInfo(0).IsName("GrenadierMeleeAttack")
                    || AIRoby.Roby_Animator.IsInTransition(0)) return;

                AIRoby.SwitchState(RobyStates.MeleeAttack);

            }
        }
        else
        {
            if (AIRoby.IsMaITooFar(AIRoby.Mai_PlayerBattleZone))
            {
                AIRoby.SwitchState(RobyStates.Follow);
            }
            else
            {
                if (AIRoby.Roby_Animator.GetCurrentAnimatorStateInfo(0).IsName("RangeStart")
                    || AIRoby.Roby_Animator.IsInTransition(0) || AIRoby.Roby_Animator.GetCurrentAnimatorStateInfo(0).IsName("RangeAttackEnd"))
                    return;

                AIRoby.SwitchState(RobyStates.RangeAttack);
            }
        }


    }

    public void CustomCollisionEnter(Script_Roby AiRoby, Collision other)
    {
    }
}
