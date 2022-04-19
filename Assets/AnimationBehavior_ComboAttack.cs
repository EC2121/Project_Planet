using UnityEngine;
public class AnimationBehavior_ComboAttack : StateMachineBehaviour
{
    private bool canCombo;
    private readonly int PlayerComboInputHash = Animator.StringToHash("ComboInput");
    private readonly int ComboAttackHash = Animator.StringToHash("ComboAttack");
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        canCombo = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (canCombo && animator.GetBool(PlayerComboInputHash) && !animator.IsInTransition(0))
        {
            canCombo = false;
            animator.SetBool(PlayerComboInputHash, false);
            if (stateInfo.IsName("Ellen_Combo4")) return;

            animator.SetTrigger(ComboAttackHash);
        }
    }
}
