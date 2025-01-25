using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MeleeStateMachine : StateManager<MeleeStateMachine.EMeleeStateMachine>
{
    public enum EMeleeStateMachine
    {
        STANCE,
        ASSUMING,
        INITIATING,
        IMPACT,
        INTERRUPT,
        NEUTRAL
    }
    /*

    Requirements:
    Weapon Selected - > Inventory
    Weapon Determines STANCE and how many hands

    Add In Combat to CharacterActions MAYBE lock on instead
    MoveSelectWheel - > Assume point, End Point
    Reset Assume and End Points after ASSUMING and INITIATING STARTS

    STANCE:
        Inventory Access - > Weapon

    ASSUMING:
        Start of Swing position

    INITIATING:
        A Current Ik Position
        An End Point

    */


    [SerializeField] Inventory inventory;
    [SerializeField] bool lockOn = false;
    [SerializeField] bool twoHand = false;
    [SerializeField] Vector3 swingStart;
    [SerializeField] Vector3 swingEnd;


    [SerializeField] CharacterActions characterActions;
    [SerializeField] MeleeContext _context;
    [SerializeField] private TwoBoneIKConstraint _leftIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIKConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;

    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private CapsuleCollider _rootCollider;
    [SerializeField] private LayerMask _layerMask; // Assign specific layers in the Inspector


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
    characterActions,
    inventory,
    swingStart,
    swingEnd,
    lockOn,
    twoHand
);

        // ConstructEnvironmentDetectionCollider();
        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(EMeleeStateMachine.STANCE, new StanceState(_context, EMeleeStateMachine.STANCE));                //Keep hands in stance position
        States.Add(EMeleeStateMachine.ASSUMING, new AssumingState(_context, EMeleeStateMachine.ASSUMING));          //Move to start of attack
        States.Add(EMeleeStateMachine.INITIATING, new InitiatingState(_context, EMeleeStateMachine.INITIATING));    //Initiate Attack 
        States.Add(EMeleeStateMachine.IMPACT, new ImpactState(_context, EMeleeStateMachine.IMPACT));                //When you hit something
        States.Add(EMeleeStateMachine.INTERRUPT, new InterruptState(_context, EMeleeStateMachine.INTERRUPT));       //When something hits you
        States.Add(EMeleeStateMachine.NEUTRAL, new NeutralState(_context, EMeleeStateMachine.NEUTRAL));       //When something hits you

        CurrentState = States[EMeleeStateMachine.NEUTRAL];
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
    }


}
