using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class EnvironmentInteractionState : BaseState<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    public float _movingOffset = .05f;
    protected EnvironmentInteractionContext Context;
    bool _shouldReset;
    public EnvironmentInteractionState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(estate)
    {
        Context = context;
    }

    protected bool CheckShouldReset()
    {
        if (_shouldReset)
        {
            Context.LowestDistance = Mathf.Infinity;
            _shouldReset = false;
            return true;
        }
        bool isPlayerStopped = Context.Rb.velocity == Vector3.zero;
        bool isMovingAway = CheckIsMovingAway();
        bool isBadAngle = CheckIsBadAngle();
        bool isPlayerJumping = Mathf.Round(Context.Rb.velocity.y) >= 1;

        if (isPlayerStopped || isMovingAway || isBadAngle || isPlayerJumping)
        {
            Context.LowestDistance = Mathf.Infinity;
            return true;
        }

        return false;
    }
    protected bool CheckIsBadAngle()
    {
        if (Context.CurrentIntersectingCollider == null)
        {
            return false;
        }
        Vector3 targetDirection = Context.ClosestPointOnColliderFromShoulder - Context.CurrentShoulderTransform.position;
        Vector3 shoulderDiretion = Context.CurrentBodySide == EnvironmentInteractionContext.EBodySide.RIGHT ?
        Context.RootTransform.right : -Context.RootTransform.right;

        float dotProduct = Vector3.Dot(shoulderDiretion, targetDirection.normalized);
        bool badAngle = dotProduct < 0;

        return badAngle;
    }

    protected bool CheckIsMovingAway()
    {
        float currentDistanceToTarget = Vector3.Distance(Context.RootTransform.position, Context.ClosestPointOnColliderFromShoulder);

        bool isSearching = Context.CurrentIntersectingCollider == null;
        if (isSearching)
        {
            return false;
        }

        bool isGettingCloser = currentDistanceToTarget <= Context.LowestDistance;
        if (isGettingCloser)
        {
            Context.LowestDistance = currentDistanceToTarget;
            return false;
        }

        bool isMovingAway = currentDistanceToTarget > Context.LowestDistance + _movingOffset;
        if (isMovingAway)
        {
            Context.LowestDistance = Mathf.Infinity;
            return true;
        }

        return false;
    }

    private Vector3 GetClosestPointOnCollider(Collider intersectingCollider, Vector3 positionToCheck)
    {
        return intersectingCollider.ClosestPoint(positionToCheck);
    }

    protected void StartIkTargetPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider.gameObject.layer == LayerMask.NameToLayer("Environment") && Context.CurrentIntersectingCollider == null)
        {
            Context.CurrentIntersectingCollider = intersectingCollider;
            Vector3 closestPointFromRoot = GetClosestPointOnCollider(intersectingCollider, Context.RootTransform.position);
            Context.SetCurrentSide(closestPointFromRoot);

            SetIkTargetPosition();
        }
    }

    protected void UpdateIkTargetPosition(Collider intersectingCollider)
    {
        if (intersectingCollider == Context.CurrentIntersectingCollider)
        {
            SetIkTargetPosition();
        }
    }

    protected void ResetIkTargetPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider == Context.CurrentIntersectingCollider)
        {
            Context.CurrentIntersectingCollider = null;
            Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
            _shouldReset = true;
        }
    }

    private void SetIkTargetPosition()
    {
        Context.ClosestPointOnColliderFromShoulder = GetClosestPointOnCollider(Context.CurrentIntersectingCollider,
        new Vector3(Context.CurrentShoulderTransform.position.x, Context.CharacterShoulderHeight, Context.CurrentShoulderTransform.position.z));

        Vector3 rayDirection = Context.CurrentShoulderTransform.position - Context.ClosestPointOnColliderFromShoulder;
        Vector3 normalizedRayDirection = rayDirection.normalized;
        float offsetDistance = 0.05f;
        Vector3 offset = normalizedRayDirection * offsetDistance;

        Vector3 offsetPosition = Context.ClosestPointOnColliderFromShoulder + offset;
        Context.CurrentIkTargetTransform.position = new Vector3(offsetPosition.x, Context.InteractionPointYOffset, offsetPosition.z);
    }
}
