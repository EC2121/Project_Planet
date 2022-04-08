using System;
using UnityEngine;

public class AI_Chompies_AttackState : AI_Enemies_IBaseState
{
   

    public void OnEnter(Enemy owner)
    {
        owner.Anim.SetBool(owner.InPursuitHash, false);
        owner.Agent.ResetPath();
    }
   
    public void OnExit(Enemy owner)
    {
    }
    public void UpdateState(Enemy owner)
    {
        float distance = Vector3.Distance(owner.transform.position, owner.Target.position);
        if (distance >= owner.AttackRange + 1)
        {
            owner.SwitchState(EnemyStates.Follow);
            return;
        }
        if (!owner.Anim.IsInTransition(0) && owner.Anim.GetCurrentAnimatorStateInfo(0).IsName("PrepareAttack"))
        {
            Attack(owner);
            return;
        }
        if (Vector3.Distance(owner.transform.position, owner.PatrolCenter) > 20 && Vector3.Distance(owner.Target.position, owner.transform.position) > 11)
        {
            owner.SwitchState(EnemyStates.Idle);
            return;
        }

    }


    private void Attack(Enemy owner)
    {
        owner.AttackTimer += Time.deltaTime;
        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation,
        Quaternion.LookRotation((owner.Target.position - owner.transform.position).normalized, Vector3.up), Time.deltaTime * 5f);
        if (owner.AttackTimer >= owner.AttackCD)
        {
            owner.AttackTimer = 0;
            owner.Anim.SetTrigger(owner.AttackHash);
            //FindNewPoint(owner);
        }
    }

    //private void FindNewPoint(Enemy owner)
    //{
    //    Vector3 random = UnityEngine.Random.insideUnitSphere;
    //    random.y = owner.transform.position.y;
    //    owner.Agent.CalculatePath(owner.Target.position + random * 3,owner.AgentPath);
    //    owner.Agent.path = owner.AgentPath;
    //}

    public void OnTrigEnter(Enemy owner, Collider other)
    {
        
    }

    public void OnCollEnter(Enemy owner, Collision other)
    {
        if (other.gameObject.CompareTag("Roby") && owner.IsAttacking)
        {
            Script_Roby.Roby_Hit.Invoke(20);
            Player_State_Machine.hit.Invoke(true);
        }
        
    }
    
}
