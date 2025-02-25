using UnityEngine;

public class ResetAttackParameter : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("attackDirection", -1); // Reset AttackIndex when the animation ends
    }
}
