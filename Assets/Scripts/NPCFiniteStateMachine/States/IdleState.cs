using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    protected override void OnEnter()
    {
        if (!machine.CheckNPCActivation())
            machine.ActivateNPC();
        machine.SetAnimatorTrigger("Stop");
        machine.StopNavigation();
        machine.BeginIdling();
    }

    protected override void OnUpdate()
    {
        if (machine.CheckIdleFinished())
        {
            if (!machine.CheckPathFinished())
            {
                machine.StopRotatingNPC();
                machine.ChangeState(machine.moveState);
            }

            else
                machine.ChangeState(machine.finalState);
        }
        else
        {
            machine.InvokeIdleFunction();
            if (machine.RetriggerConversation())
                machine.ChangeState(machine.talkState);
        }
    }

    protected override void OnExit()
    {
        machine.ResetAnimTrigger("Stop");
        //machine.StopRotatingNPC();
    }
}
