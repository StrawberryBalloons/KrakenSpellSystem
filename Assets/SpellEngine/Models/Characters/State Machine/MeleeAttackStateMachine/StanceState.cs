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
        stancePlaceholder = new Vector3(0.13f, 1.031f, 0.411f);

        if (Context._twoHand)
        {
            Context.leftIKConstraint.data.target.localPosition = new Vector3(0.902f, 0.409f, -0.081f);
        }

        _elapsedTime = 0.0f;

    }

    public override void ExitState()
    {
        Debug.Log("Exiting Stance State");
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
        //Ik weights to 1
        if (_elapsedTime < 3f)
        {
            //Move Ik Targets to Ready Position
            Context.leftIKConstraint.weight = Mathf.Lerp(Context.leftIKConstraint.weight, targetWeight, _elapsedTime / _lerpDuration);
            Context.leftMultiRotationConstraint.weight = Mathf.Lerp(Context.leftMultiRotationConstraint.weight, targetWeight, _elapsedTime / _lerpDuration);

            Context.rightIKConstraint.weight = Mathf.Lerp(Context.rightIKConstraint.weight, targetWeight, _elapsedTime / _lerpDuration);
            Context.rightMultiRotationConstraint.weight = Mathf.Lerp(Context.rightMultiRotationConstraint.weight, targetWeight, _elapsedTime / _lerpDuration);
        }

        Context.rightIKConstraint.data.target.localPosition = Vector3.Lerp(Context.rightIKConstraint.transform.localPosition, stancePlaceholder, _elapsedTime / _lerpDuration);
        if (Context._twoHand)
        {
            Context.leftIKConstraint.data.target.localPosition = Vector3.Lerp(Context.leftIKConstraint.transform.localPosition, stancePlaceholder, _elapsedTime / _lerpDuration);
        }
    }

    public override MeleeStateMachine.EMeleeStateMachine GetNextState()
    {
        //Wait for melee input to go to ASSUMING
        if (Input.GetMouseButtonDown(1)) //replace with interact/use key
        {
            Context.leftIKConstraint.weight = targetWeight;
            Context.leftMultiRotationConstraint.weight = targetWeight;
            Context.rightIKConstraint.weight = targetWeight;
            Context.rightMultiRotationConstraint.weight = targetWeight;

            return MeleeStateMachine.EMeleeStateMachine.ASSUMING;
        }

        //Go to NEUTRAL stance if target not locked or no weapon drawn
        if (_elapsedTime >= 60f)
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