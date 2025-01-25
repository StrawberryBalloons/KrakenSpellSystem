using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class MeleeState : BaseState<MeleeStateMachine.EMeleeStateMachine>
{
    protected MeleeContext Context;
    public MeleeState(MeleeContext context, MeleeStateMachine.EMeleeStateMachine estate) : base(estate)
    {
        Context = context;
    }


}
