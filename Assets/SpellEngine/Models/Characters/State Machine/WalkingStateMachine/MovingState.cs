using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MovingState : WalkingState
{
    public bool leftStepperMoving { get; private set; }
    public bool rightLegStepperMoving { get; private set; }
    Coroutine moveCoroutine;

    public MovingState(WalkingContext context, WalkingStateMachine.EWalkingStateMachine estate)
      : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Entering Moving State");
        Context.StartStepping();

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
    }

    public override WalkingStateMachine.EWalkingStateMachine GetNextState()
    {
        if (!Context._characterActions.ReturnIsGrounded())
        {
            return WalkingStateMachine.EWalkingStateMachine.Falling;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}