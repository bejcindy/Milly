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
        machine.ResetAnimTrigger("Move");
        if(!machine.CheckSpecialIdleAnim() && !machine.CheckRemainInAnim())
            machine.SetAnimatorTrigger("Stop");
        else if(machine.CheckSpecialIdleAnim() && !machine.CheckRemainInAnim())
        {
            machine.SetSpecialIdleAnim();
        }

        if (machine.CheckNavOn())
        {
            machine.StopNavigation();
        }

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
 