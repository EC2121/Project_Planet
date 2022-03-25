using UnityEngine;

public class AI_Chompies_HitState : AI_Enemies_IBaseState
{
    public void OnEnter(Enemy owner)
    {
        owner.Anim.SetFloat("HorizontalHitDot", owner.HorizontalDot);
        owner.Anim.SetTrigger("Hit");
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





}
