using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AssumingState : MeleeState
{
    float _elapsedTime = 0.0f;
    float _lerpDuration = 0.5f;  // How long it takes to transition hands

    Vector3 _initialRightHandLocalPos; // Right hand's start position in LOCAL space
    Vector3 _initialLeftHandLocalPos;  // Left hand's start position in LOCAL space

    Vector3 _currentRightHandLocalPos; // Right hand's current position in LOCAL space
    Vector3 _currentLeftHandLocalPos;  // Left hand's current position in LOCAL space

    Vector3 endPosRight; // Right hand's end position
    Vector3 endPosLeft;  // Left hand's end position

    float _totalDistanceRight; // Tracks how close the right hand is to the target
    float _totalDistanceLeft;  // Tracks how close the left hand is to the target

    float _initialWeight = 1f; // Starts as fully procedural
    float targetWeight = 0f;   // Ends as fully animation-controlled

    public AssumingState(MeleeContext context, MeleeStateMachine.EMeleeStateMachine estate)
      : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Entering Assuming State");

        _elapsedTime = 0.0f; // Reset timer

        // Store initial positions in local space
        _initialRightHandLocalPos = Context.rightIKConstraint.data.target.localPosition;
        _initialLeftHandLocalPos = Context.leftIKConstraint.data.target.localPosition;

        // Procedural weight starts at 100%
        Context.leftIKConstraint.weight = _initialWeight;
        Context.leftMultiRotationConstraint.weight = _initialWeight;
        Context.rightIKConstraint.weight = _initialWeight;
        Context.rightMultiRotationConstraint.weight = _initialWeight;

        // Trigger the attack animation in the animator
        Context._animator.SetTrigger("AttackTrigger");
        //Multiply the animation speed to 0 to stop it from moving (Pause the animation on frame 1)
        Context._animator.SetFloat("AttackAnimationSpeed", 0);
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Assuming State");
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(_elapsedTime / _lerpDuration); // Normalize lerp time

        // Get current hand positions in local space
        _currentRightHandLocalPos = Context.rightIKConstraint.data.target.localPosition;
        _currentLeftHandLocalPos = Context.leftIKConstraint.data.target.localPosition;

        if (t > 0.1) //Hard limit on when the calculations can start so it doesn't do the exact same values
        {
            // Calculate end positions based on lerp percentage
            endPosRight = _initialRightHandLocalPos + (_currentRightHandLocalPos - _initialRightHandLocalPos) * t;
            endPosLeft = _initialLeftHandLocalPos + (_currentLeftHandLocalPos - _initialLeftHandLocalPos) * t;
        }

        // Smoothly transition procedural weight to animation weight
        Context.leftIKConstraint.weight = Mathf.Lerp(_initialWeight, targetWeight, t);
        Context.leftMultiRotationConstraint.weight = Mathf.Lerp(_initialWeight, targetWeight, t);

        Context.rightIKConstraint.weight = Mathf.Lerp(_initialWeight, targetWeight, t);
        Context.rightMultiRotationConstraint.weight = Mathf.Lerp(_initialWeight, targetWeight, t);
    }

    public override MeleeStateMachine.EMeleeStateMachine GetNextState()
    {
        // Define how close the hands need to be to their targets
        float threshold = 0.01f;

        // Calculate the distance to the right and left hand targets
        float distanceRight = Vector3.Distance(endPosRight, _currentRightHandLocalPos);
        float distanceLeft = Vector3.Distance(endPosLeft, _currentLeftHandLocalPos);

        // If both hands are close enough to their targets, transition to INITIATING
        if (distanceRight < threshold && distanceLeft < threshold)
        {
            return MeleeStateMachine.EMeleeStateMachine.INITIATING;
        }

        return StateKey; // Stay in ASSUMING if not close enough
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
