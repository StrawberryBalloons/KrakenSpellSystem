using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MovingState : WalkingState
{
    public bool leftStepperMoving { get; private set; }
    public bool rightLegStepperMoving { get; private set; }
    Coroutine moveCoroutine;
    float _targetWeight = 1f;
    float _elapsedTime = 0f;
    float _lerpDuration = 1f;

    public MovingState(WalkingContext context, WalkingStateMachine.EWalkingStateMachine estate)
      : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Entering Moving State");
        Context.StartStepping();
        _elapsedTime = 0f;
        // set ik targets to parentless
        // Context._leftStepper.SetParentNull();
        // Context._rightStepper.SetParentNull();
    }

    public override void ExitState()
    {
        // set ik targets parents
        //transform.SetParent(null);

        Debug.Log("Exiting Moving State");
        Context.StopStepping();
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;

        Context.LeftIKConstraint.weight = Mathf.Lerp(Context.LeftIKConstraint.weight, _targetWeight, _elapsedTime / _lerpDuration);
        Context.RightIKConstraint.weight = Mathf.Lerp(Context.RightIKConstraint.weight, _targetWeight, _elapsedTime / _lerpDuration);

        Context.leftMultiRotationConstraint.weight = Mathf.Lerp(Context.leftMultiRotationConstraint.weight, _targetWeight, _elapsedTime / _lerpDuration);
        Context.rightMultiRotationConstraint.weight = Mathf.Lerp(Context.rightMultiRotationConstraint.weight, _targetWeight, _elapsedTime / _lerpDuration);
    }

    public override WalkingStateMachine.EWalkingStateMachine GetNextState()
    {
        if (!Context._characterActions.ReturnIsGrounded())
        {
            return WalkingStateMachine.EWalkingStateMachine.Falling;
        }
        if (Context.Rb.velocity.magnitude > 5f)
        {
            return WalkingStateMachine.EWalkingStateMachine.Animated;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}