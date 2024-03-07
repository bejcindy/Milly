using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    protected override void OnEnter()
    {
        if (!machine.IsIdling())
        {
            machine.BeginIdling();
            machine.SetIdleRotation();
            machine.PlayIdleAnimation();
        }
        else
        {
            machine.ResumeIdling();
            machine.ResumeIdleAnimation();
        }

    }
    
    protected override void OnUpdate()
    {
        if (!machine.IsIdling())
        {
            machine.StopRotation();
            machine.DelayMove();
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
        machine.SetMainTalkTrue();
    }
}
 