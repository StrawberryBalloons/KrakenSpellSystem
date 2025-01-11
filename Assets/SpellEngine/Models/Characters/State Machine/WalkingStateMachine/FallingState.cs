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
        // Lerp the position of __leftStepper and _rightStepper IK targets to their home positions
        if (Context._leftStepper != null && Context._rightStepper != null)
        {
            // Interpolation speed
            float lerpSpeed = 5f * Time.deltaTime;

            // Lerp left stepper
            if (leftTransform != null && leftHomeTransform != null)
            {
                leftTransform.position = Vector3.Lerp(
                    leftTransform.position,
                    leftHomeTransform.position + (leftHomeTransform.up / 2),
                    lerpSpeed
                );
            }

            // Lerp right stepper
            if (rightTransform != null && rightHomeTransform != null)
            {
                rightTransform.position = Vector3.Lerp(
                    rightTransform.position,
                    rightHomeTransform.position + (rightHomeTransform.up / 2),
                    lerpSpeed
                );
            }
        }
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