using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    protected override void OnEnter()
    {
        //if (!machine.CheckNPCActivation())
        //    machine.ActivateNPC();

        if(!machine.CheckSpecialIdleAnim())
            machine.SetAnimatorTrigger("Stop");
        else
        {
            machine.SetSpecialIdleAnim();
        }

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
                machine.SetMainTalkTrue();
                machine.ChangeState(machine.moveState);
            }

            else
                machine.ChangeState(machine.finalState);
        }
        else
        {
            machine.InvokeIdleFunction();
            if (machine.CheckInConversation())
            {
                machine.ChangeState(machine.talkState);
            }

        }
    }

    protected override void OnExit()
    {
        //if(!machine.CheckSpecialIdleAnim())
        //    machine.ResetAnimTrigger("Stop");
        //machine.StopRotatingNPC();
    }
}
