using UnityEngine;

public class Random_Idle : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("IdleIndex",Random.Range(0,4));
    }
}
