using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WalkingContext : MonoBehaviour
{

    private TwoBoneIKConstraint _leftIKConstraint;
    private TwoBoneIKConstraint _rightIKConstraint;
    private MultiRotationConstraint _leftMultiRotationConstraint;
    private MultiRotationConstraint _rightMultiRotationConstraint;

    public Rigidbody _rigidbody;
    private CapsuleCollider _rootCollider;
    private Transform _rootTransform;
    public void Initialize(
        TwoBoneIKConstraint leftIKConstraint,
        TwoBoneIKConstraint rightIKConstraint,
        MultiRotationConstraint leftMultiRotationConstraint,
        MultiRotationConstraint rightMultiRotationConstraint,
        Rigidbody rigidbody,
        CapsuleCollider rootCollider,
        Transform rootTransform,
        LayerMask layer,
        WalkingStepper leftStepper,
        WalkingStepper rightStepper,
        bool legSteppingEnabled,
        CharacterActions characterActions)
    {
        _leftIKConstraint = leftIKConstraint;
        _rightIKConstraint = rightIKConstraint;
        _leftMultiRotationConstraint = leftMultiRotationConstraint;
        _rightMultiRotationConstraint = rightMultiRotationConstraint;
        _rigidbody = rigidbody;
        _rootCollider = rootCollider;
        _rootTransform = rootTransform;
        layerMask = layer;
        _leftStepper = leftStepper;
        _rightStepper = rightStepper;
        _legSteppingEnabled = legSteppingEnabled;
        _characterActions = characterActions;

        CharacterHipHeight = leftIKConstraint.data.root.transform.position.y;
    }
    public WalkingContext(TwoBoneIKConstraint leftIKConstraint, TwoBoneIKConstraint rightIKConstraint, MultiRotationConstraint leftMultiRotationConstraint, MultiRotationConstraint rightMultiRotationConstraint, Rigidbody rigidbody, CapsuleCollider rootCollider
        , Transform rootTransform, LayerMask layer, WalkingStepper leftStepper, WalkingStepper rightStepper, bool legSteppingEnabled)
    {
        _leftIKConstraint = leftIKConstraint;
        _rightIKConstraint = rightIKConstraint;

        _leftMultiRotationConstraint = leftMultiRotationConstraint;
        _rightMultiRotationConstraint = rightMultiRotationConstraint;

        _rigidbody = rigidbody;
        _rootCollider = rootCollider;
        _rootTransform = rootTransform;

        layerMask = layer;

        _leftStepper = leftStepper;
        _rightStepper = rightStepper;

        _legSteppingEnabled = legSteppingEnabled;

        CharacterHipHeight = leftIKConstraint.data.root.transform.position.y;
        // SetCurrentSide(Vector3.positiveInfinity);
    }
    public CharacterActions _characterActions;
    public bool _legSteppingEnabled;
    public WalkingStepper _leftStepper;
    public WalkingStepper _rightStepper;
    public LayerMask layerMask; // Assign specific layers in the Inspector
    public TwoBoneIKConstraint LeftIKConstraint => _leftIKConstraint;
    public TwoBoneIKConstraint RightIKConstraint => _rightIKConstraint;
    public MultiRotationConstraint leftMultiRotationConstraint => _leftMultiRotationConstraint;
    public MultiRotationConstraint rightMultiRotationConstraint => _rightMultiRotationConstraint;

    public Rigidbody Rb => _rigidbody;
    public CapsuleCollider RootCollider => _rootCollider;
    public Transform RootTransform => _rootTransform;
    public float CharacterHipHeight { get; private set; }
    public Coroutine StepRoutine;

    IEnumerator LegUpdateCoroutine()
    {
        while (true)
        {
            while (!_legSteppingEnabled) yield return null;

            do
            {
                _leftStepper.TryMove();
                yield return null;
            } while (_leftStepper.Moving);

            do
            {
                _rightStepper.TryMove();
                yield return null;
            } while (_rightStepper.Moving);
        }
        yield return null;
    }



    public void StartStepping()
    {
        // Validate dependencies
        if (_leftStepper == null || _rightStepper == null)
        {
            Debug.LogError("Stepper references (_leftStepper or _rightStepper) are not assigned in WalkingContext!");
            return;
        }

        if (!_legSteppingEnabled)
        {
            Debug.LogWarning("Leg stepping is disabled. Ensure _legSteppingEnabled is true before starting stepping.");
            return;
        }

        if (StepRoutine == null)
        {
            _leftStepper.SetParentsNull();
            _rightStepper.SetParentsNull();
            Debug.Log("Starting leg stepping coroutine...");
            StepRoutine = StartCoroutine(LegUpdateCoroutine());
        }
        else
        {
            Debug.LogWarning("Stepping routine is already running.");
        }
    }

    public void StopStepping()
    {
        if (StepRoutine != null)
        {
            StopCoroutine(StepRoutine);
            StepRoutine = null;
            _leftStepper.ResetParents();
            _leftStepper.transform.localPosition = Vector3.zero;

            _rightStepper.ResetParents();
            _rightStepper.transform.localPosition = Vector3.zero;
        }
    }

}
