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
        if (Vector3.Distance(owner.transform.position, owner.PatrolCenter) > 30 && Vector3.Distance(owner.Target.position, owner.transform.position) > 11)
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
        }
    }


    public void OnTrigEnter(Enemy owner, Collider other)
    {
        
    }

    public void OnCollEnter(Enemy owner, Collision other)
    {
        if (other.gameObject.CompareTag("Player") && owner.IsAttacking)
        {
            Player_State_Machine.hit.Invoke();
            Debug.Log("CESTODENTROOOOOOO");
        }

    }
    
}
