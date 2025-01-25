using System.Collections;
using UnityEngine;

public class AnimatedState : WalkingState
{
    private Vector3 leftTargetPosition;
    private Vector3 rightTargetPosition;
    private Vector3 centerPosition;

    private float transitionTime = 0f;
    private float maxTransitionTime = 0.5f; // Time to move between points, adjust as needed
    private int currentPointIndex = 0;

    private Vector3[] leftTargetPositions = new Vector3[3];
    private Vector3[] rightTargetPositions = new Vector3[3];

    private Rigidbody Rb; // Assuming this is the Rigidbody of the character
    private Transform homeTransform; // Assuming this is the character's transform
    private Transform PlayerTransform; // Assuming this is the player's transform
    private LayerMask groundRaycastMask; // Assuming this is the mask for raycasting to the ground
    private bool isGrounded; // Assuming you have a way to check this (e.g., through a method or context)

    private bool isMovingRight; // This will track whether we move to the right or left based on input

    float _targetWeight = 0f;
    float _elapsedTime = 0f;
    float _lerpDuration = 1f;

    public AnimatedState(WalkingContext context, WalkingStateMachine.EWalkingStateMachine estate)
        : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Entering Animated");

        // Determine foot movement based on the direction of input
        // DetermineFootMovementDirection();

        // CalculateTargetPositions();
        // leftTargetPosition = leftTargetPositions[0];
        // rightTargetPosition = rightTargetPositions[0];
        // transitionTime = 0f;
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Animated");
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;

        Context.LeftIKConstraint.weight = Mathf.Lerp(Context.LeftIKConstraint.weight, _targetWeight, _elapsedTime / _lerpDuration);
        Context.RightIKConstraint.weight = Mathf.Lerp(Context.RightIKConstraint.weight, _targetWeight, _elapsedTime / _lerpDuration);

        Context.leftMultiRotationConstraint.weight = Mathf.Lerp(Context.leftMultiRotationConstraint.weight, _targetWeight, _elapsedTime / _lerpDuration);
        Context.rightMultiRotationConstraint.weight = Mathf.Lerp(Context.rightMultiRotationConstraint.weight, _targetWeight, _elapsedTime / _lerpDuration);

        // Update transition time
        // transitionTime += Time.deltaTime;

        // // Lerp between current position and target position
        // float t = Mathf.Clamp01(transitionTime / maxTransitionTime);
        // Context.LeftIKConstraint.data.root.localPosition = Vector3.Lerp(Context.LeftIKConstraint.data.root.localPosition, leftTargetPosition, t);
        // Context.RightIKConstraint.data.root.localPosition = Vector3.Lerp(Context.RightIKConstraint.data.root.localPosition, rightTargetPosition, t);

        // // Check if transition is complete
        // if (t >= 1f)
        // {
        //     // Move to the next target position
        //     currentPointIndex = (currentPointIndex + 1) % leftTargetPositions.Length;
        //     leftTargetPosition = leftTargetPositions[currentPointIndex];
        //     rightTargetPosition = rightTargetPositions[currentPointIndex];
        //     transitionTime = 0f;
        // }
    }

    public override WalkingStateMachine.EWalkingStateMachine GetNextState()
    {
        if (!Context._characterActions.ReturnIsGrounded())
        {
            return WalkingStateMachine.EWalkingStateMachine.Falling;
        }
        if (Context.Rb.velocity.magnitude < 5f)
        {
            return WalkingStateMachine.EWalkingStateMachine.Moving;
        }
        return StateKey;
    }

    // private void CalculateTargetPositions()
    // {
    //     // Calculate the foot placement based on movement direction
    //     leftTargetPositions[0] = UpdateFootPlacement();
    //     leftTargetPositions[1] = -leftTargetPositions[0]; // Example: Flip target for the other foot

    //     // Centered and slightly raised position
    //     leftTargetPositions[2] = Context.transform.position + Vector3.up * 0.1f; // Adjust height as needed


    //     rightTargetPositions[0] = leftTargetPositions[1];
    //     rightTargetPositions[1] = leftTargetPositions[2];
    //     rightTargetPositions[2] = leftTargetPositions[0];
    // }

    // private void DetermineFootMovementDirection()
    // {
    //     // Get input direction in local space
    //     float horizontalInput = Input.GetAxis("Horizontal");

    //     // Determine the direction of movement
    //     if (horizontalInput > 0)
    //     {
    //         isMovingRight = true;
    //     }
    //     else if (horizontalInput < 0)
    //     {
    //         isMovingRight = false;
    //     }
    // }

    // Vector3 UpdateFootPlacement()
    // {
    //     if (!isGrounded)
    //     {
    //         return Vector3.zero;
    //     }

    //     float rayDistance = 1.05f;

    //     // Smoothly interpolate velocity factor for raycast debugging
    //     float minFactor = 1.2f; // Max factor at low velocity
    //     float maxFactor = 0.4f; // Min factor at high velocity

    //     float minSpeed = 5f;
    //     float maxSpeed = 20f;
    //     float minDuration = 0.4f;
    //     float maxDuration = 0.1f;

    //     float velocityNormalFactor = Mathf.Lerp(minFactor, maxFactor, Mathf.Clamp01(Rb.velocity.magnitude / maxSpeed));
    //     float velocity = Rb.velocity.magnitude;
    //     float moveDuration = Mathf.Lerp(minDuration, maxDuration, Mathf.InverseLerp(minSpeed, maxSpeed, velocity));

    //     // Get input direction in local space
    //     float horizontalInput = Input.GetAxis("Horizontal");
    //     float verticalInput = Input.GetAxis("Vertical");
    //     Vector3 localInputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

    //     // Convert local input direction to world space
    //     Vector3 worldInputDirection = PlayerTransform.TransformDirection(localInputDirection);

    //     // Perform raycast to determine foot placement
    //     Vector3 rayOrigin = homeTransform.position + (worldInputDirection / velocityNormalFactor) + homeTransform.up;
    //     if (velocity > 10f)
    //     {
    //         rayOrigin += (homeTransform.forward / 3);
    //     }
    //     Vector3 rayDirection = Vector3.down;

    //     // Draw debug ray
    //     Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);

    //     if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayDistance, groundRaycastMask))
    //     {
    //         // Return the hit point converted to local space
    //         return homeTransform.InverseTransformPoint(hit.point);
    //     }
    //     else
    //     {
    //         // If no hit, return the local input direction
    //         return localInputDirection;
    //     }
    // }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
