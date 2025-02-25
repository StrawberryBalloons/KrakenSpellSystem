using UnityEngine;

public class PauseAnimationOnEnter : StateMachineBehaviour
{
    // Called when the animation state is entered
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 0; // Pause the animation
    }
}
