using UnityEngine;

public class AI_Chompies_ThrownState : AI_Enemies_IBaseState
{
    public void OnEnter(Enemy owner)
    {
        owner.Anim.SetTrigger("Hit");
        owner.Anim.SetTrigger("Thrown");
    }
    public void OnExit(Enemy owner)
    {

    }
    public void UpdateState(Enemy owner)
    {
       
    }
   
    public void OnTrigEnter(Enemy owner, Collider other)
    {
        

    }

    public void OnCollEnter(Enemy owner, Collision other)
    {
    }
}
