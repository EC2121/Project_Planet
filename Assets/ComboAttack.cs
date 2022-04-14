using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    private bool canCombo;
    private int PlayerComboInputHash = Animator.StringToHash("PlayerComboInput");
    private int ComboAttackHash = Animator.StringToHash("ComboAttack");
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        canCombo = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (canCombo && animator.GetBool(PlayerComboInputHash) && !animator.IsInTransition(3))
        {
            canCombo = false;
            animator.SetBool(PlayerComboInputHash, false);
            if (stateInfo.IsName("Ellen_Combo4")) return;
         
            animator.SetTrigger(ComboAttackHash);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
