using UnityEngine;
using UnityEngine.SceneManagement;

public class AI_Chompies_DieState : AI_Enemies_IBaseState
{
    public void OnCollEnter(Enemy owner, Collision other)
    {
    }

    public void OnEnter(Enemy owner)
    {
        owner.Agent.destination = owner.transform.position;
        owner.Anim.SetTrigger(owner.DieHash);

        if (SceneManager.GetActiveScene().name != "Scene_AlienTest_2") return;
        Script_WorldSwap.DeadChompys++;
    }
    public void OnExit(Enemy owner)
    {

    }

    public void OnTrigEnter(Enemy owner, Collider other)
    {
    }

    public void UpdateState(Enemy owner)
    {

    }







}
