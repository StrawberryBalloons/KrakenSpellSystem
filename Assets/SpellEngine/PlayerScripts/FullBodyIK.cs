using UnityEngine;

public class FullBodyIK : MonoBehaviour
{
    [Header("References")]
    public Animator animator; // Reference to the Animator component

    [Header("IK Targets")]
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public Transform headLookTarget;

    [Header("Weights")]
    [Range(0, 1)] public float leftHandWeight = 0f;
    [Range(0, 1)] public float rightHandWeight = 0f;
    [Range(0, 1)] public float leftFootWeight = 1.0f;
    [Range(0, 1)] public float rightFootWeight = 1.0f;
    [Range(0, 1)] public float headLookWeight = 1.0f;

    [Header("Foot Rotation Weights")]
    [Range(0, 1)] public float leftFootRotationWeight = 1.0f;
    [Range(0, 1)] public float rightFootRotationWeight = 1.0f;

    void OnAnimatorIK(int layerIndex)
    {
        if (!animator) return;

        // Helper method to override only the Y-axis
        Vector3 OverrideYPosition(Vector3 original, Vector3 target)
        {
            return new Vector3(original.x, target.y, original.z);
        }

        // Left Hand IK
        if (leftHandTarget)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, OverrideYPosition(animator.GetIKPosition(AvatarIKGoal.LeftHand), leftHandTarget.position));
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }

        // Right Hand IK
        if (rightHandTarget)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, OverrideYPosition(animator.GetIKPosition(AvatarIKGoal.RightHand), rightHandTarget.position));
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }

        // Left Foot IK
        if (leftFootTarget)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootRotationWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, OverrideYPosition(animator.GetIKPosition(AvatarIKGoal.LeftFoot), leftFootTarget.position));
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootTarget.rotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
        }

        // Right Foot IK
        if (rightFootTarget)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootRotationWeight);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, OverrideYPosition(animator.GetIKPosition(AvatarIKGoal.RightFoot), rightFootTarget.position));
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootTarget.rotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
        }

        // Head Look IK
        if (headLookTarget)
        {
            animator.SetLookAtWeight(headLookWeight);
            animator.SetLookAtPosition(headLookTarget.position);
        }
        else
        {
            animator.SetLookAtWeight(0);
        }
    }
}
