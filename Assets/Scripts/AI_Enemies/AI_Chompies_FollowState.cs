using UnityEngine;

public class AI_Chompies_FollowState : AI_Chompies_BaseState
{
    public override void OnEnter(AI_Chompies_MGR AI)
    {
        AI.Owner.SetPathToPlayer();
    }

    public override void OnExit(AI_Chompies_MGR AI)
    {

    }

    public override void OnTriggerEnter(AI_Chompies_MGR AI, Collider collider)
    {

    }

    public override void UpdateState(AI_Chompies_MGR AI)
    {
        AI.Owner.FollowPlayer();
        if (AI.Owner.CanAttackTarget())
        {
            AI.SwitchState(AI.attackState);
            return;
        }
        if (AI.Owner.CheckDistance())
        {
            AI.Owner.Res();
            AI.SwitchState(AI.idleState);
            return;
        }

    }
}
