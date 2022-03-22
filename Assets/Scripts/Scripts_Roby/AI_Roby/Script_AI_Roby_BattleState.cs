using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AI_Roby_BattleState : Script_AI_Roby_BaseState
{
    public override void OnEnter(Script_AI_Roby_MGR AIRoby)
    {
        //AIRoby.Owner.ChaseTarget();
    }

    public override void OnExit(Script_AI_Roby_MGR AIRoby)
    {
    }

    public override void CustomOnTriggerEnter(Script_AI_Roby_MGR AiRoby, Collider collider)
    {
        AiRoby.Owner.EnemysInArea(collider.gameObject);
    }

    public override void CustomOnTriggerExit(Script_AI_Roby_MGR AiRoby, Collider collider)
    {
        AiRoby.Owner.EnemyOutArea(collider.gameObject);
    }

    public override void CustomOnTriggerStay(Script_AI_Roby_MGR AiRoby, Collider collider)
    {
        AiRoby.Owner.EnemysInArea(collider.gameObject);
    }

    public override void UpdateState(Script_AI_Roby_MGR AIRoby)
    {
        print("Battle");

        if (!AIRoby.Owner.AreEnemyNear())
        {
            AIRoby.SwitchState(AIRoby.AIRobyIdle);
            return;
        }

        AIRoby.Owner.ChooseTarget();

        if (AIRoby.Owner.EnemyWithinRange())
        {
            if (AIRoby.Owner.IsMaITooFar(AIRoby.Owner.mai_PlayerBattleZone))
            {

                if (AIRoby.Owner.roby_Animator.GetCurrentAnimatorStateInfo(0).IsName("ZoneAttack")
                    || AIRoby.Owner.roby_Animator.IsInTransition(0)) return;
                print("ZoneAttack");

                AIRoby.Owner.RobyZoneAttackTrigger();
            }
            else
            {

                if (AIRoby.Owner.roby_Animator.GetCurrentAnimatorStateInfo(0).IsName("GrenadierMeleeAttack")
                    || AIRoby.Owner.roby_Animator.IsInTransition(0)) return;

                print("Melee");
                AIRoby.Owner.ChaseTarget();

                if (AIRoby.Owner.CheckRemainingDistance())
                {
                    AIRoby.Owner.StopRoby();
                    AIRoby.Owner.RobyMeleeAttack();
                }
            }
        }
        else
        {
            if (AIRoby.Owner.IsMaITooFar(AIRoby.Owner.mai_PlayerBattleZone))
            {
                print("Run");

                AIRoby.SwitchState(AIRoby.AiRobyFollowState);
                AIRoby.ignoreEnemys = true;
            }
            else
            {
                if (AIRoby.Owner.roby_Animator.GetCurrentAnimatorStateInfo(0).IsName("New StateMachine")
                    || AIRoby.Owner.roby_Animator.IsInTransition(0)) return;

                print("Range");

                AIRoby.Owner.RobyRangeAttack();
            }
        }
    }


}
