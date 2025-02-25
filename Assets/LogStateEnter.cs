using UnityEngine;

public class LogStateEnter : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        string stateName = animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateInfo.shortNameHash.ToString())
            ? animator.GetCurrentAnimatorStateInfo(layerIndex).ToString()
            : "Unknown State";

        Debug.Log("Enter State: " + stateName);
    }
}
