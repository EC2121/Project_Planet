using UnityEngine;

public class AI_Chompies_AttackState : AI_Chompies_BaseState
{
    public override void OnEnter(AI_Chompies_MGR AI)
    {
        AI.Owner.StopChasingPlayer();
    }

    public override void OnExit(AI_Chompies_MGR AI)
    {

    }

    public override void OnTriggerEnter(AI_Chompies_MGR AI, Collider collider)
    {

    }

    public override void UpdateState(AI_Chompies_MGR AI)
    {
        if (!AI.Owner.IsAttacking())
        {
            AI.Owner.Attack();
        }
        if (!AI.Owner.CanAttackTarget() && !AI.Owner.IsAttacking())
        {
            AI.SwitchState(AI.followState);
        }
    }
}
