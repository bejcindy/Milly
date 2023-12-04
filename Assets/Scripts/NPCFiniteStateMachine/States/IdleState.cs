using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    protected override void OnEnter()
    {
        //if (!machine.CheckNPCActivation())
        //    machine.ActivateNPC();
        machine.SetIdleRotation();
        if(!machine.CheckSpecialIdleAnim())
            machine.SetAnimatorTrigger("Stop");
        else
        {
            machine.SetSpecialIdleAnim();
        }

        machine.StopNavigation();
        if (!machine.GetIdling())
            machine.BeginIdling();
        else
            machine.StartIdling();
    }
    
    protected override void OnUpdate()
    {

        if (machine.CheckNotIdling())
        {
            if (!machine.CheckPathFinished())
            {
                machine.StopRotation();
                machine.SetMainTalkTrue();



                machine.ChangeState(machine.moveState);
            }

        }
        else
        {
            machine.InvokeIdleFunction();
            if (machine.CheckInConversation() && !machine.GetMainTalkTrue())
            {

                machine.ChangeState(machine.talkState);
            }

        }
    }

    protected override void OnExit()
    {

    }
}
 