using UnityEngine;
using UnityEditor;
using System.Collections;
public class Script_AI_Roby_Dead : Script_AI_Roby_BaseState
{
    //public GameObject Roby_GuiInteractWrite;
    private AnimationState AnimationStateDead;

    public void CustomCollisionEnter(Script_Roby AiRoby, Collision other) { }

    public void CustomOnTriggerEnter(Script_Roby AiRoby, Collider collider)
    {
        //if (collider.CompareTag("Player"))
        //    Roby_GuiInteractWrite.SetActive(true);
    }

    public void CustomOnTriggerExit(Script_Roby AiRoby, Collider collider)
    {
        //if (collider.CompareTag("Player"))
        //    Roby_GuiInteractWrite.SetActive(false);
    }

    public void CustomOnTriggerStay(Script_Roby AiRoby, Collider collider)
    {
        //if (collider.CompareTag("Player") && !Roby_GuiInteractWrite.activeInHierarchy) Roby_GuiInteractWrite.SetActive(true);
    }

    public void OnExit(Script_Roby AIRoby) { }

    public void UpdateState(Script_Roby AIRoby)
    {
        //if (Roby_GuiInteractWrite.activeInHierarchy)
        //    Roby_GuiInteractWrite.transform.position = Camera.main.WorldToScreenPoint(AIRoby.Roby_BoxCollider_Alive.bounds.center + ( Vector3.up * 0.5f ));

        AnimationStateDead.time -= Time.deltaTime;
    }

    public void OnEnter(Script_Roby AIRoby)
    {
        AIRoby.Roby_BoxCollider_Alive.enabled = false;
        AIRoby.Roby_SphereCollider_Alive.enabled = false;
        AIRoby.Roby_NavAgent.enabled = false;

        AIRoby.Roby_NavMeshObstacle.enabled = true;
        AIRoby.Roby_SphereCollider_Dead.enabled = true;
        AIRoby.Roby_Animator.SetTrigger(AIRoby.Roby_AshAnimator_Dead);

    }


}
