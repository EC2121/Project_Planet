using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Dead : Player_BaseState
{
    private float timeToEnd=5;

    public Player_Dead(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
        //  InitializeSubState();
    }

    public override void EnterState()
    {
        Context.Animator.SetBool("isDead", true);
        Context.Animator.SetBool(Context.IsWalkingHash, false);
        Context.Animator.SetBool(Context.IsRunningHash, false);
        Context.CharacterController.enabled = false;
        Context.CharacterController.radius = 0;
        Context.CharacterController.height = 0;

        //Context.AppliedMovementY = 0;
    }

    public override void UpdateState()
    {
        timeToEnd -= Time.deltaTime;

        Context.CurrentMovementX = 0;
        Context.CurrentMovementZ = 0;
        Context.CurrentMovementY = 0;
        // Context.AppliedMovementX = 0;
        //Context.AppliedMovementZ = 0;



        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.Log("TEESTTTTTTTTTTTTT");
    }

    public override void CheckSwitchStates()
    {
        //if (Context.Hp > 0)
        //{
        //    SwitchState(Factory.Grounded());
        //}
        if (timeToEnd <= 0)
        {
            SceneManager.LoadScene("UI_MenuScene");
        }
    }

    public override void InitializeSubState()
    {
    }
}