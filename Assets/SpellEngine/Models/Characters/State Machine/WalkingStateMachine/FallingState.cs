using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FallingState : WalkingState
{
    Transform leftTransform;
    Transform leftHomeTransform;

    Transform rightTransform;
    Transform rightHomeTransform;

    float _targetWeight = 0f;
    float _elapsedTime = 0f;
    float _lerpDuration = 1f;

    public FallingState(WalkingContext context, WalkingStateMachine.EWalkingStateMachine estate)
      : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Entering Moving State");
        leftTransform = Context._leftStepper.GetStepperTransform();
        leftHomeTransform = Context._leftStepper.GetStepperHomeTransform();

        rightTransform = Context._rightStepper.GetStepperTransform();
        rightHomeTransform = Context._rightStepper.GetStepperHomeTransform();

        leftTransform.SetParent(leftHomeTransform);
        rightTransform.SetParent(rightHomeTransform);
    }

    public override void ExitState()
    {
        // set ik targets parents
        leftTransform.SetParent(null);
        rightTransform.SetParent(null);

        Debug.Log("Exiting Moving State");
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
        if (Context._characterActions.ReturnIsGrounded())
        {
            //Do Function before moving back to moving state, go to landing state then go to moving
            return WalkingStateMachine.EWalkingStateMachine.Moving;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}