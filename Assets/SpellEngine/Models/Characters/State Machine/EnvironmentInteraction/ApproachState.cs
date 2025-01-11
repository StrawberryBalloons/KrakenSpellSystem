using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ApproachState : EnvironmentInteractionState
{
    float _elapsedTime = 0f;
    float _lerpDuration = 5f;
    float _approachDuration = 2f;
    float _approachWeight = 0.5f;
    float _approachRotationWeight = .75f;
    float _rotationSpeed = 500f;
    float _riseDistanceThreshold = 0.5f;
    public ApproachState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base
    (context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }

    public override void EnterState()
    {
        // Debug.Log("Entering APPROACH State");
        _elapsedTime = 0f;
    }
    public override void ExitState() { }
    public override void UpdateState()
    {
        //may need to replace the up with gameObject.transform.up 
        Quaternion expectedGroundRotation = Quaternion.LookRotation(-Vector3.up, Context.RootTransform.forward);

        Context.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIkTargetTransform.rotation, expectedGroundRotation, _rotationSpeed * Time.deltaTime);
        _elapsedTime += Time.deltaTime;

        Context.CurrentIkConstraint.weight = Mathf.Lerp(Context.CurrentIkConstraint.weight, _approachWeight, _elapsedTime / _lerpDuration);


        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight, _approachRotationWeight, _elapsedTime / _lerpDuration);
    }
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        bool isOverStateLifeDuration = _elapsedTime >= _approachDuration;
        if (isOverStateLifeDuration)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Reset;
        }

        bool isWithinArmsReach = Vector3.Distance(Context.ClosestPointOnColliderFromShoulder, Context.CurrentShoulderTransform.position) < _riseDistanceThreshold;

        bool isClosestPointOnColliderReal = Context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;

        if (isClosestPointOnColliderReal && isWithinArmsReach)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Rise;
        }
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other)
    {
        StartIkTargetPositionTracking(other);
    }
    public override void OnTriggerStay(Collider other)
    {
        UpdateIkTargetPosition(other);
    }
    public override void OnTriggerExit(Collider other)
    {
        ResetIkTargetPositionTracking(other);
    }
}
