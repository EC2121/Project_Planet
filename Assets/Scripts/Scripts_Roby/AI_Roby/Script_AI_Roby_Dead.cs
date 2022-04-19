using UnityEngine;
public class Script_AI_Roby_Dead : Script_AI_Roby_BaseState
{

    public void CustomCollisionEnter(Script_Roby AiRoby, Collision other) { }

    public void CustomOnTriggerEnter(Script_Roby AiRoby, Collider collider)
    {
        if (collider.CompareTag("Player"))
            AiRoby.Roby_Interact_TXT.SetActive(true);
    }

    public void CustomOnTriggerExit(Script_Roby AiRoby, Collider collider)
    {
        if (collider.CompareTag("Player"))
            AiRoby.Roby_Interact_TXT.SetActive(false);
    }

    public void CustomOnTriggerStay(Script_Roby AiRoby, Collider collider)
    {
        if (collider.CompareTag("Player") && !AiRoby.Roby_Interact_TXT.activeInHierarchy) AiRoby.Roby_Interact_TXT.SetActive(true);
    }

    public void OnExit(Script_Roby AIRoby)
    {
        AIRoby.Roby_NavMeshObstacle.enabled = false;
        AIRoby.Roby_SphereCollider_Dead.enabled = false;
        AIRoby.Roby_Interact_TXT.SetActive(false);

        AIRoby.roby_Life = 200;
        AIRoby.RobyHpSlider.value = AIRoby.roby_Life;

        AIRoby.Roby_BoxCollider_Alive.enabled = true;
        AIRoby.Roby_SphereCollider_Alive.enabled = true;
        AIRoby.Roby_NavAgent.enabled = true;
    }

    public void UpdateState(Script_Roby AIRoby)
    {
        if (AIRoby.Roby_Interact_TXT.activeInHierarchy)
        {
            AIRoby.Roby_Interact_TXT.transform.position = Camera.main.WorldToScreenPoint(AIRoby.Roby_BoxCollider_Alive.bounds.center + ( Vector3.up ));

            AIRoby.Mai_Player.GetComponent<Player_State_Machine>().reviveSlider.transform.position = Camera.main.WorldToScreenPoint(AIRoby.Roby_BoxCollider_Alive.bounds.center + ( Vector3.up *1.5f));
        }

        if (AIRoby.roby_FullSlider)
        {
            AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_StartRepair);
            AIRoby.SwitchState(RobyStates.Idle);
        }

    }
    public void OnEnter(Script_Roby AIRoby)
    {
        AIRoby.Roby_BoxCollider_Alive.enabled = false;
        AIRoby.Roby_SphereCollider_Alive.enabled = false;
        AIRoby.Roby_NavAgent.enabled = false;
        AIRoby.RobyHpSlider.value = 0;

        AIRoby.Roby_NavMeshObstacle.enabled = true;
        AIRoby.Roby_SphereCollider_Dead.enabled = true;
        AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_Dead);
    }


}
