using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCFSM;

public class State
{
    public BaseStateMachine machine; 

    public void OnStateEnter(BaseStateMachine stateController)
    {
        // Code placed here will always run
        machine = stateController;
        OnEnter();
    }
    protected virtual void OnEnter()
    {
        // Code placed here can be overridden
    }
    public void OnStateUpdate()
    {
        // Code placed here will always run
        OnUpdate();
    }
    protected virtual void OnUpdate()
    {
        // Code placed here can be overridden
    }
    public void OnStateExit()
    {
        // Code placed here will always run
        OnExit();
    }
    protected virtual void OnExit()
    {
        // Code placed here can be overridden
    }
}


