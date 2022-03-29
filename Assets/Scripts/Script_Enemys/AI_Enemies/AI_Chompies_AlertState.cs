using UnityEngine;

public class AI_Chompies_AlertState : AI_Enemies_IBaseState
{
 

    public void OnEnter(Enemy owner)
    {
        owner.Agent.destination = owner.transform.position;
        owner.Anim.SetBool("Spotted",true);
        owner.Anim.SetBool(owner.HasTargetHash, true);
        AlertOthers(owner, owner.IsAlerted);
    }

    

    public void OnExit(Enemy owner)
    {
        owner.IsAlerted = false;
        owner.Anim.SetBool("Spotted", false);
        //owner.Anim.SetBool(owner.SpottedHash, false);
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

            Enemy enemyComponent = enemy.GetComponent<Enemy>();
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

    


}
