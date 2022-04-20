using System;
using UnityEngine;

public class AI_Chompies_FollowState : AI_Enemies_IBaseState
{

    public void OnEnter(Enemy owner)
    {
        owner.Anim.SetBool(owner.InPursuitHash, true);
        owner.Anim.SetBool(owner.HasTargetHash, true);
    }


    public void OnExit(Enemy owner)
    {
    }

    public void UpdateState(Enemy owner)
    {
        float distance = Vector3.Distance(owner.transform.position, owner.Target.position);

        if (Vector3.Distance(owner.transform.position, owner.PatrolCenter) > 30 && Vector3.Distance(owner.Target.position,owner.transform.position) > 11)
        {
            owner.SwitchState(EnemyStates.Idle);
            return;
        }

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
    }

    public void OnTrigEnter(Enemy owner, Collider other)
    {
    }

    public void OnCollEnter(Enemy owner, Collision other)
    {
    }
}