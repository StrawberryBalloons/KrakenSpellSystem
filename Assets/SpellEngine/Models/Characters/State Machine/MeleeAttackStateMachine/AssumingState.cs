using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AssumingState : MeleeState
{
    float _elapsedTime = 0.0f;
    float _lerpDuration = 0.5f;
    public AssumingState(MeleeContext context, MeleeStateMachine.EMeleeStateMachine estate)
      : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Entering Assuming State");
        _elapsedTime = 0.0f;
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Assuming State");
    }

    public override void UpdateState()
    {
        //Move Ik Targets to Ready Position
        _elapsedTime += Time.deltaTime;
        Vector3 leftLocal = Context.leftIKConstraint.data.target.localPosition;
        Vector3 rightLocal = Context.rightIKConstraint.data.target.localPosition;
        // float t = Mathf.Clamp01(_elapsedTime / _lerpDuration); // Normalize the elapsed time

        Context.rightIKConstraint.data.target.localPosition = Vector3.Lerp(rightLocal, Context._swingStart, _elapsedTime / _lerpDuration);
        if (Context._twoHand)
        {
            Context.leftIKConstraint.data.target.localPosition = Vector3.Lerp(leftLocal, Context._swingStart, _elapsedTime / _lerpDuration);
        }

    }

    public override MeleeStateMachine.EMeleeStateMachine GetNextState()
    {

        //INTERRUPT if hit

        //INITIATING when targets are in position
        if (_elapsedTime > _lerpDuration)
        {
            return MeleeStateMachine.EMeleeStateMachine.INITIATING;
        }

        //stay in current state
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}