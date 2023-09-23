using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    protected override void OnEnter()
    {
        machine.SetAnimatorTrigger("Stop");
        machine.StopNavigation();
        machine.BeginIdling();
    }

    protected override void OnUpdate()
    {
        if (machine.CheckIdleFinished())
        {
            if (!machine.CheckPathFinished())
                machine.ChangeState(machine.moveState);
            else
                machine.ChangeState(machine.finalState);
        }
        else
        {
            machine.RotateTowards(machine.GetCurrentDestination());
            machine.InvokeIdleFunction();
            if (machine.RetriggerConversation())
                machine.ChangeState(machine.talkState);
        }
    }

    protected override void OnExit()
    {
        machine.ResetAnimTrigger("Stop");
        
    }
}
