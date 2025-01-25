using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WalkingStateMachine : StateManager<WalkingStateMachine.EWalkingStateMachine>
{
    public enum EWalkingStateMachine
    {
        Moving,
        Falling,
        Animated,
    }

    [SerializeField] CharacterActions characterActions;
    [SerializeField] WalkingContext _context;
    [SerializeField] WalkingStepper leftStepper;
    [SerializeField] WalkingStepper rightStepper;
    [SerializeField] bool legStepperEnabled;

    [SerializeField] private TwoBoneIKConstraint _leftIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIKConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;

    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private CapsuleCollider _rootCollider;
    [SerializeField] private LayerMask _layerMask; // Assign specific layers in the Inspector

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // if (_context != null && _context.ClosestPointOnColliderFromShoulder != null)
        // {
        //     Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, 0.3f);
        // }
    }

    void Awake()
    {
        // _context = new WalkingContext(_leftIKConstraint, _rightIKConstraint, _leftMultiRotationConstraint, _rightMultiRotationConstraint, _rigidBody, _rootCollider,
        //  transform.root, _layerMask, leftStepper, rightStepper, legStepperEnabled);
        _context.Initialize(
    _leftIKConstraint,
    _rightIKConstraint,
    _leftMultiRotationConstraint,
    _rightMultiRotationConstraint,
    _rigidBody,
    _rootCollider,
    transform.root,
    _layerMask,
    leftStepper,
    rightStepper,
    legStepperEnabled,
    characterActions
);

        // ConstructEnvironmentDetectionCollider();
        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(EWalkingStateMachine.Moving, new MovingState(_context, EWalkingStateMachine.Moving));
        States.Add(EWalkingStateMachine.Falling, new FallingState(_context, EWalkingStateMachine.Falling));
        States.Add(EWalkingStateMachine.Animated, new AnimatedState(_context, EWalkingStateMachine.Animated));

        CurrentState = States[EWalkingStateMachine.Moving];
    }


}
