using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class WalkingState : BaseState<WalkingStateMachine.EWalkingStateMachine>
{
    protected WalkingContext Context;
    public WalkingState(WalkingContext context, WalkingStateMachine.EWalkingStateMachine estate) : base(estate)
    {
        Context = context;
    }


}
