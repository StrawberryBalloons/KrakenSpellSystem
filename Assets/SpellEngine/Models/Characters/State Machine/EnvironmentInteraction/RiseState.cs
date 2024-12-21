using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RiseState : EnvironmentInteractionState
{
    float _elapsedTime = 0f;
    float _lerpDuration = 5f;
    float _riseWeight = 1f;
    Quaternion _expectedRotation;
    float _maxDistance = .5f;
    protected LayerMask _interactableLayerMask = LayerMask.GetMask("Environment");
    float _rotationSpeed = 1000f;
    float _touchDistanceThreshold = 0.5f;
    float _touchTimeThreshold = 1f;

    public RiseState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base
(context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Entering RISE State");
        _touchTimeThreshold = Random.Range(1f, 10f);
        _elapsedTime = 0f;
    }
    public override void ExitState() { }
    public override void UpdateState()
    {
        CalculateExpectedHandRotation();

        Context.InteractionPointYOffset = Mathf.Lerp(Context.InteractionPointYOffset, Context.ClosestPointOnColliderFromShoulder.y, _elapsedTime / _lerpDuration);

        Context.CurrentIkConstraint.weight = Mathf.Lerp(Context.CurrentIkConstraint.weight, _riseWeight, _elapsedTime / _lerpDuration);

        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight, _riseWeight, _elapsedTime / _lerpDuration);

        Context.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIkTargetTransform.rotation, _expectedRotation, _rotationSpeed * Time.deltaTime);



        _elapsedTime += Time.deltaTime;
    }
    public void AddCube(Vector3 position, string name)
    {
        // Create a new cube at the given position
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Set the cube's position to the specified position
        cube.transform.position = position;

        // Set the cube's scale to 0.1 in all axes
        cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        // Set the cube's name
        cube.name = name;
        // Disable the collider to make the cube non-collidable
        Collider collider = cube.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }
    private void CalculateExpectedHandRotation() //may need to replace the vector3.up with gameobject.transform.up
    {
        Vector3 startPos = Context.CurrentShoulderTransform.position;
        Vector3 endPos = Context.ClosestPointOnColliderFromShoulder;
        Vector3 direction = (endPos - startPos).normalized;

        RaycastHit hit;
        if (Physics.Raycast(startPos, direction, out hit, _maxDistance, _interactableLayerMask))
        {
            Vector3 surfaceNormal = hit.normal;
            Vector3 targetForward = -surfaceNormal;
            _expectedRotation = Quaternion.LookRotation(targetForward, Vector3.up);
        }
    }
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        if (CheckShouldReset())
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Reset;
        }

        if (Vector3.Distance(Context.CurrentIkTargetTransform.position, Context.ClosestPointOnColliderFromShoulder) < _touchDistanceThreshold
        && _elapsedTime >= _touchTimeThreshold)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Touch;
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
