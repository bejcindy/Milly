using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : State
{

    protected override void OnEnter()
    {
        machine.SetAnimatorTrigger("Stop");
    }

    protected override void OnUpdate()
    {
        if(machine.CheckFollowPlayer() && machine.CheckReachDestination())
        {
            if (machine.CheckPlayerMove())
            {
                machine.ChangeState(machine.moveState);
            }
        }
        else if (!machine.CheckFollowPlayer())
        {
            machine.ResetPath();
            machine.ChangeState(machine.moveState);
        }

        if (machine.CheckInConversation())
        {
            machine.ChangeState(machine.talkState);
        }

    }

    protected override void OnExit()
    {
        //machine.ResetAnimTrigger("Idle");
    }
}
