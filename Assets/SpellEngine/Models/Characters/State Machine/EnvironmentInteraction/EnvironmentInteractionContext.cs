using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionContext
{

    public enum EBodySide
    {
        RIGHT,
        LEFT
    }


    private TwoBoneIKConstraint _leftIKConstraint;
    private TwoBoneIKConstraint _rightIKConstraint;
    private MultiRotationConstraint _leftMultiRotationConstraint;
    private MultiRotationConstraint _rightMultiRotationConstraint;

    public Rigidbody _rigidbody;
    private CapsuleCollider _rootCollider;
    private Transform _rootTransform;
    private Vector3 _leftOriginalTargetPosition;
    private Vector3 _rightOriginalTargetPosition;

    public EnvironmentInteractionContext(TwoBoneIKConstraint leftIKConstraint, TwoBoneIKConstraint rightIKConstraint, MultiRotationConstraint leftMultiRotationConstraint,
    MultiRotationConstraint rightMultiRotationConstraint, Rigidbody rigidbody, CapsuleCollider rootCollider, Transform rootTransform)
    {
        _leftIKConstraint = leftIKConstraint;
        _rightIKConstraint = rightIKConstraint;
        _leftMultiRotationConstraint = leftMultiRotationConstraint;
        _rightMultiRotationConstraint = rightMultiRotationConstraint;
        _rigidbody = rigidbody;
        _rootCollider = rootCollider;
        _rootTransform = rootTransform;
        _leftOriginalTargetPosition = _leftIKConstraint.data.target.transform.localPosition;
        _rightOriginalTargetPosition = _rightIKConstraint.data.target.transform.localPosition;
        OriginalTargetRotation = _leftIKConstraint.data.target.rotation;

        CharacterShoulderHeight = leftIKConstraint.data.root.transform.position.y;
        SetCurrentSide(Vector3.positiveInfinity);
    }

    public TwoBoneIKConstraint LeftIKConstraint => _leftIKConstraint;
    public TwoBoneIKConstraint rightIKConstraint => _rightIKConstraint;
    public MultiRotationConstraint leftMultiRotationConstraint => _leftMultiRotationConstraint;
    public MultiRotationConstraint rightMultiRotationConstraint => _rightMultiRotationConstraint;
    public Rigidbody Rb => _rigidbody;
    public CapsuleCollider RootCollider => _rootCollider;
    public Transform RootTransform => _rootTransform;

    public float CharacterShoulderHeight { get; private set; }

    public Collider CurrentIntersectingCollider { get; set; }
    public TwoBoneIKConstraint CurrentIkConstraint { get; private set; }
    public MultiRotationConstraint CurrentMultiRotationConstraint { get; private set; }
    public Transform CurrentIkTargetTransform { get; private set; }
    public Transform CurrentShoulderTransform { get; private set; }
    public EBodySide CurrentBodySide { get; private set; }
    public Vector3 ClosestPointOnColliderFromShoulder { get; set; } = Vector3.positiveInfinity;
    public float InteractionPointYOffset { get; set; } = 0f;
    public float ColliderCenterY { get; set; }
    public Vector3 CurrentOriginalTargetPosition { get; private set; }
    public Quaternion OriginalTargetRotation { get; private set; }
    public float LowestDistance { get; set; } = Mathf.Infinity;

    public void SetCurrentSide(Vector3 positionToCheck)
    {
        Vector3 leftShoulder = _leftIKConstraint.data.root.transform.position;
        Vector3 rigthShoulder = rightIKConstraint.data.root.transform.position;

        bool isLeftCloser = Vector3.Distance(positionToCheck, leftShoulder) < Vector3.Distance(positionToCheck, rigthShoulder);

        if (isLeftCloser)
        {
            CurrentBodySide = EBodySide.LEFT;
            CurrentIkConstraint = _leftIKConstraint;
            CurrentMultiRotationConstraint = leftMultiRotationConstraint;
            CurrentOriginalTargetPosition = _leftOriginalTargetPosition;
        }
        else
        {
            CurrentBodySide = EBodySide.RIGHT;
            CurrentIkConstraint = _rightIKConstraint;
            CurrentMultiRotationConstraint = rightMultiRotationConstraint;
            CurrentOriginalTargetPosition = _rightOriginalTargetPosition;
        }

        CurrentShoulderTransform = CurrentIkConstraint.data.root.transform;
        CurrentIkTargetTransform = CurrentIkConstraint.data.target.transform;

    }

}
