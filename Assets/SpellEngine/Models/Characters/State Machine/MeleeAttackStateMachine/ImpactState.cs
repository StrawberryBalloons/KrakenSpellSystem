using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ImpactState : MeleeState
{

    public ImpactState(MeleeContext context, MeleeStateMachine.EMeleeStateMachine estate)
      : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Entering Impact State");
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Impact State");
    }

    public override void UpdateState()
    {
        //Get colliding object

        //add that object to "hit" list in context

        //determine the damage for it

        //determine if it's a "hard" target

        //if it's a "hard" target don't continue with initating?
    }

    public override MeleeStateMachine.EMeleeStateMachine GetNextState()
    {
        //Move back to INITIATING

        //INTERRUPT if hit, but after calcs

        //stay in current
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}