using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> CurrentState;

    protected bool IsTrantitioningState = false;

    void Start()
    {
        CurrentState.EnterState();
    }

    void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (nextStateKey.Equals(CurrentState.StateKey) && !IsTrantitioningState)
        {
            CurrentState.UpdateState();
        }
        else
        {
            TransitionToState(nextStateKey);
        }
    }

    public void TransitionToState(EState stateKey)
    {
        IsTrantitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        IsTrantitioningState = false;
    }

    void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }
    void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }
    void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }
    void OnCollisionEnter() { }
    void OnCollisionStay() { }
    void OnCollisionExit() { }
}
