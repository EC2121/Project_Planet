using System;
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
        //owner.Anim.SetBool(owner.InPursuitHash, false);
        //owner.Anim.SetBool(owner.HasTargetHash, false);
    }

    public void UpdateState(Enemy owner)
    {
        float distance = Vector3.Distance(owner.transform.position, owner.Target.position);

        //
        // foreach (var enemy in nearEnemies)
        // {
        //     float distance2 = Vector3.Distance(enemy.transform.position, owner.Target.position);
        //     if (distance2 > 6)
        //     {
        //         owner.SwitchState(EnemyStates.Idle);
        //     }
        // }
        // if (distance > 6)
        // {
        //     Collider[] nearEnemies = Physics.OverlapSphere(owner.transform.position, owner.AlertRange, 1 << 6);
        //
        //     owner.isEnemyTofar = true;
        //
        //     foreach (Collider enemy in nearEnemies)
        //     {
        //         Enemy enemyComponent = enemy.GetComponentInParent<Enemy>();
        //
        //         if (!enemyComponent.isEnemyTofar) return;
        //         
        //         enemyComponent.SwitchState(EnemyStates.Idle);
        //     }
        // }
        // if (!owner.IsAlerted )
        // {
        //     owner.Target = null;
        //     owner.SwitchState(EnemyStates.Idle);
        //
        //     return;
        // }
        if (Vector3.Distance(owner.transform.position, owner.PatrolCenter) > 20 && Vector3.Distance(owner.Target.position,owner.transform.position) > 11)
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
        //owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation,
        //    Quaternion.LookRotation((owner.Target.position - owner.transform.position).normalized, Vector3.up), Time.deltaTime * 5f);
    }

    public void OnTrigEnter(Enemy owner, Collider other)
    {
    }

    public void OnCollEnter(Enemy owner, Collision other)
    {
    }
}