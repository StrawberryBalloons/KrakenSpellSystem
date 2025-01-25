using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MeleeContext : MonoBehaviour
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
        CharacterActions characterActions,
        Inventory inventory,
        Vector3 swingStart,
        Vector3 swingEnd,
        bool lockOn,
        bool twoHand)
    {
        _leftIKConstraint = leftIKConstraint;
        _rightIKConstraint = rightIKConstraint;
        _leftMultiRotationConstraint = leftMultiRotationConstraint;
        _rightMultiRotationConstraint = rightMultiRotationConstraint;
        _rigidbody = rigidbody;
        _rootCollider = rootCollider;
        _rootTransform = rootTransform;
        layerMask = layer;
        _characterActions = characterActions;
        _inventory = inventory;
        _swingStart = swingStart;
        _swingEnd = swingEnd;
        _lockOn = lockOn;
        _twoHand = twoHand;
    }
    // public MeleeContext(TwoBoneIKConstraint leftIKConstraint, TwoBoneIKConstraint rightIKConstraint, MultiRotationConstraint leftMultiRotationConstraint, MultiRotationConstraint rightMultiRotationConstraint, Rigidbody rigidbody, CapsuleCollider rootCollider
    //     , Transform rootTransform, LayerMask layer, CharacterActions characterActions, Inventory inventory, Vector3 swingStart, Vector3 swingEnd, bool lockOn)
    // {
    //     _leftIKConstraint = leftIKConstraint;
    //     _rightIKConstraint = rightIKConstraint;

    //     _leftMultiRotationConstraint = leftMultiRotationConstraint;
    //     _rightMultiRotationConstraint = rightMultiRotationConstraint;

    //     _rigidbody = rigidbody;
    //     _rootCollider = rootCollider;
    //     _rootTransform = rootTransform;

    //     layerMask = layer;

    //     _characterActions = characterActions;
    //     _inventory = inventory;
    //     _swingStart = swingStart;
    //     _swingEnd = swingEnd;
    //     _lockOn = lockOn;

    //     // SetCurrentSide(Vector3.positiveInfinity);
    // }


    public bool _twoHand;
    public bool _lockOn; //change to taken damage or dealt damage recently, work in character actions
    public Vector3 _swingStart;
    public Vector3 _swingEnd;
    public Inventory _inventory;
    public CharacterActions _characterActions;
    public LayerMask layerMask; // Assign specific layers in the Inspector
    public TwoBoneIKConstraint leftIKConstraint => _leftIKConstraint;
    public TwoBoneIKConstraint rightIKConstraint => _rightIKConstraint;
    public MultiRotationConstraint leftMultiRotationConstraint => _leftMultiRotationConstraint;
    public MultiRotationConstraint rightMultiRotationConstraint => _rightMultiRotationConstraint;

    public Rigidbody Rb => _rigidbody;
    public CapsuleCollider RootCollider => _rootCollider;
    public Transform RootTransform => _rootTransform;



}
