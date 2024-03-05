using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    protected override void OnEnter()
    {
        machine.SetIdleRotation();
        machine.PlayIdleAnimation();


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
                machine.DelayMove();
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
        machine.SetMainTalkTrue();
    }
}
 