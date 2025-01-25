using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class InterruptState : MeleeState
{

    public InterruptState(MeleeContext context, MeleeStateMachine.EMeleeStateMachine estate)
      : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Entering Interrupt State");
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Interrupt State");
    }

    public override void UpdateState()
    {
        //ragdoll, flinch, other thing?
    }

    public override MeleeStateMachine.EMeleeStateMachine GetNextState()
    {
        //move to STANCE when whatever is done here
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}