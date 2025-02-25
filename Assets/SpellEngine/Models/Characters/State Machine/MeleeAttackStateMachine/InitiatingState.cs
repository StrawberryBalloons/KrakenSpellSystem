using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class InitiatingState : MeleeState
{

    float _elapsedTime = 0.0f;
    float _lerpDuration = 0.5f;
    public InitiatingState(MeleeContext context, MeleeStateMachine.EMeleeStateMachine estate)
      : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Entering Initiating State");

        //Change the speed multiplier to 1 to get it moving again
        Context._animator.SetFloat("AttackAnimationSpeed", 1);

        // Procedural weight set to 0
        Context.leftIKConstraint.weight = 0;
        Context.leftMultiRotationConstraint.weight = 0;
        Context.rightIKConstraint.weight = 0;
        Context.rightMultiRotationConstraint.weight = 0;
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Initiating State");
    }

    public override void UpdateState()
    {
        //lerp IkTarget to strike endpoint
        //Move Ik Targets to Ready Position
        _elapsedTime += Time.deltaTime;

    }

    public override MeleeStateMachine.EMeleeStateMachine GetNextState()
    {
        //Go to IMPACT state when colliding with something not on the "hit" list

        //When IkTarget reaches destination -> STANCE
        if (_elapsedTime > _lerpDuration)
        {
            return MeleeStateMachine.EMeleeStateMachine.STANCE;
        }

        //IkTarget reaches destination and Another Attack input, being the button to trigger an attack -> Go to ASSUMING
        // if (_elapsedTime > _lerpDuration && "Context.swingStart" != null)
        // {
        //     return MeleeStateMachine.EMeleeStateMachine.ASSUMING;
        // }

        //stay in current state
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}