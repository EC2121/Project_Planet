using UnityEngine;

public class AI_Chompies_FollowState : AI_Enemies_IBaseState
{
    //public override void OnEnter(AI_Chompies_MGR AI)
    //{
    //    //AI.Owner.SetPathToPlayer();
    //}

    public void OnEnter(Enemy owner)
    {
        owner.Anim.SetBool(owner.InPursuitHash, true);
        owner.Anim.SetBool(owner.HasTargetHash, true);
    }

    //public override void OnExit(AI_Chompies_MGR AI)
    //{

    //}

    public void OnExit(Enemy owner)
    {

    }

    public void UpdateState(Enemy owner)
    {
        float distance = Vector3.Distance(owner.transform.position, owner.Target.position);
        if (distance <= owner.AttackRange)
        {
            owner.SwitchState(EnemyStates.Attack);
            return;
        }
        FollowPlayer(owner);
    }


    public void CanAttackTarget()
    {

    }

    private void FollowPlayer(Enemy owner)
    {
        owner.Agent.CalculatePath(owner.Target.position, owner.AgentPath);
        owner.Agent.path = owner.AgentPath;
        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation,
            Quaternion.LookRotation((owner.Target.position - owner.transform.position).normalized, Vector3.up), Time.deltaTime * 5f);
    }

    public void OnTrigEnter(Enemy owner, Collider other)
    {

    }
    
}
