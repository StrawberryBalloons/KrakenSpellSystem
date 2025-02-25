using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class StanceState : MeleeState
{

    float targetWeight = 1f;
    float _elapsedTime = 0.0f;
    float _lerpDuration = 0.5f;
    Vector3 stancePlaceholder;

    public StanceState(MeleeContext context, MeleeStateMachine.EMeleeStateMachine estate)
      : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Entering Stance State");
        //Set Ik position to where the stance requests it, get stance from weapon?

        //Stance plays when there is no attack direction and the weapon has been drawn

        _elapsedTime = 0.0f;

    }

    public override void ExitState()
    {
        Debug.Log("Exiting Stance State");
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
    }

    public override MeleeStateMachine.EMeleeStateMachine GetNextState()
    {
        //Wait for melee input to go to ASSUMING
        // if (Input.GetMouseButtonDown(1)) //replace with interact/use key
        if (Context._animator.GetInteger("attackDirection") > -1)
        {
            // Context.leftIKConstraint.weight = targetWeight;
            // Context.leftMultiRotationConstraint.weight = targetWeight;
            // Context.rightIKConstraint.weight = targetWeight;
            // Context.rightMultiRotationConstraint.weight = targetWeight;

            return MeleeStateMachine.EMeleeStateMachine.ASSUMING;
        }

        //Go to NEUTRAL state if target not locked or no weapon drawn (need to implement target lock at a later date)
        if (!Context._lockOn || Context._animator.GetInteger("StanceType") == -1)
        {
            return MeleeStateMachine.EMeleeStateMachine.NEUTRAL;
        }

        //Go to INTERRUPT state if hit


        //Stay in current state
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}