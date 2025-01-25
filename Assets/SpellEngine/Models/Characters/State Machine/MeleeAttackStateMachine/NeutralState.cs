using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NeutralState : MeleeState
{
    float targetWeight = 0f;
    float _elapsedTime = 0.0f;
    float _lerpDuration = 1f;


    public NeutralState(MeleeContext context, MeleeStateMachine.EMeleeStateMachine estate)
      : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Entering Neutral State");
        _elapsedTime = 0f;
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Neutral State");
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime < 3f)
        {
            //Move Ik Targets to Ready Position
            Context.leftIKConstraint.weight = Mathf.Lerp(Context.leftIKConstraint.weight, targetWeight, _elapsedTime / _lerpDuration);
            Context.leftMultiRotationConstraint.weight = Mathf.Lerp(Context.leftMultiRotationConstraint.weight, targetWeight, _elapsedTime / _lerpDuration);

            Context.rightIKConstraint.weight = Mathf.Lerp(Context.rightIKConstraint.weight, targetWeight, _elapsedTime / _lerpDuration);
            Context.rightMultiRotationConstraint.weight = Mathf.Lerp(Context.rightMultiRotationConstraint.weight, targetWeight, _elapsedTime / _lerpDuration);
        }
    }

    public override MeleeStateMachine.EMeleeStateMachine GetNextState()
    {
        //Move to STANCE if in combat or Melee Move selected
        if (Context._lockOn)
        {
            return MeleeStateMachine.EMeleeStateMachine.STANCE;
        }
        //move to INTERRUPT if hit

        //otherwise stay in NEUTRAL state
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}