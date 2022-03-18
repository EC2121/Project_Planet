using UnityEngine;

public class AI_Chompies_IdleState : AI_Chompies_BaseState
{
    public override void OnEnter(AI_Chompies_MGR AI)
    {
        AI.Owner.Idle();
    }

    public override void OnExit(AI_Chompies_MGR AI)
    {

    }

    public override void OnTriggerEnter(AI_Chompies_MGR AI, Collider collider)
    {

    }

    public override void UpdateState(AI_Chompies_MGR AI)
    {
        if (AI.Owner.CheckForPlayer())
        {
            AI.SwitchState(AI.followState);
            return;
        }

        if (AI.Owner.FindNewPoint())
        {
            AI.SwitchState(AI.patrolState);
        }
       

    }
}
