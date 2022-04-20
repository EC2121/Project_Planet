using System.Collections;
using UnityEngine;

public class AI_Chompies_AlertState : AI_Enemies_IBaseState
{
 
    IEnumerator Alert(Enemy owner)
    {
        yield return new WaitForSeconds(0.35f);
        owner.SwitchState(EnemyStates.Follow);
    }

    public void OnEnter(Enemy owner)
    {
        owner.Anim.SetBool("Spotted",true);
        owner.Anim.SetBool(owner.HasTargetHash, true);
        owner.Agent.destination = owner.transform.position;
        AlertOthers(owner, owner.IsAlerted);
        owner.StartCoroutine(Alert(owner));
    }

    

    public void OnExit(Enemy owner)
    {
        owner.IsAlerted = false;
        owner.Anim.SetBool("Spotted", false);
    }

    public void UpdateState(Enemy owner)
    {
       
    }

    private void AlertOthers(Enemy owner,bool isSource)
    {
        if (!isSource) return;

        Collider[] nearEnemies = Physics.OverlapSphere(owner.transform.position, owner.AlertRange, 1 << 6);

        foreach (var enemy in nearEnemies)
        {
            if (enemy.gameObject == owner.gameObject) continue;

            Enemy enemyComponent = enemy.GetComponentInParent<Enemy>();
            if (ReferenceEquals(enemyComponent.Target,null))
            {
                enemyComponent.SwitchState(EnemyStates.Alert);
                enemyComponent.Target = owner.Target;
            }
            
        }

    }

    public void OnTrigEnter(Enemy owner, Collider other)
    {

    }

    public void OnCollEnter(Enemy owner, Collision other)
    {
    }
}
