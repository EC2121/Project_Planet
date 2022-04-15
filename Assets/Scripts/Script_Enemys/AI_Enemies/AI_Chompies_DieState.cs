using UnityEngine;

public class AI_Chompies_DieState : AI_Enemies_IBaseState
{
    public void OnCollEnter(Enemy owner, Collision other)
    {
    }

    public void OnEnter(Enemy owner)
    {
        owner.Agent.destination = owner.transform.position;
        owner.Anim.SetTrigger(owner.DieHash);
        owner.GetComponent<Rigidbody>().detectCollisions = false;
        owner.GetComponent<Rigidbody>().useGravity = false;
        //EnemyMGR.enemies.Remove(owner.gameObject);
    }
    public void OnExit(Enemy owner)
    {

    }

    public void OnTrigEnter(Enemy owner,Collider other)
    {
    }

    public void UpdateState(Enemy owner)
    {
       
    }


  

    


}
