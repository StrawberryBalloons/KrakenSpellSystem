using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionStateMachine : StateManager<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    public enum EEnvironmentInteractionState
    {
        Search,
        Approach,
        Rise,
        Touch,
        Reset,
    }

    private EnvironmentInteractionContext _context;

    [SerializeField] private TwoBoneIKConstraint _leftIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIKConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;

    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private CapsuleCollider _rootCollider;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_context != null && _context.ClosestPointOnColliderFromShoulder != null)
        {
            Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, 0.3f);
        }
    }

    void Awake()
    {
        _context = new EnvironmentInteractionContext(_leftIKConstraint, _rightIKConstraint, _leftMultiRotationConstraint, _rightMultiRotationConstraint, _rigidBody, _rootCollider, transform.root);

        ConstructEnvironmentDetectionCollider();
        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(EEnvironmentInteractionState.Reset, new ResetState(_context, EEnvironmentInteractionState.Reset));
        States.Add(EEnvironmentInteractionState.Search, new SearchState(_context, EEnvironmentInteractionState.Search));
        States.Add(EEnvironmentInteractionState.Approach, new ApproachState(_context, EEnvironmentInteractionState.Approach));
        States.Add(EEnvironmentInteractionState.Rise, new RiseState(_context, EEnvironmentInteractionState.Rise));
        States.Add(EEnvironmentInteractionState.Touch, new TouchState(_context, EEnvironmentInteractionState.Touch));

        CurrentState = States[EEnvironmentInteractionState.Reset];
    }

    private void ConstructEnvironmentDetectionCollider()
    {
        Debug.Log("Adding Box Collider");
        float wingspan = _rootCollider.height;
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_rootCollider.center.x, _rootCollider.center.y + (.25f * wingspan) + 1, _rootCollider.center.z + (.5f * wingspan));
        boxCollider.isTrigger = true;

        _context.ColliderCenterY = _rootCollider.center.y;

        // Set the BoxCollider to the Player layer
        int layer = LayerMask.NameToLayer("UI");
        if (layer != -1) // Ensure the layer exists
        {
            gameObject.layer = layer;
        }
    }

}
