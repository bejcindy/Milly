using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected override void OnEnter()
    {
        if (!machine.CheckFollowPlayer())
        {
            if (!machine.CheckReachDestination())
                machine.BeginNavigation();
            else
                machine.SetNPCDestination();
        }

        else
            machine.FollowPlayer();
        machine.BeginNavigation();
        machine.SetAnimatorTrigger("Move");
    }

    protected override void OnUpdate()
    {
        if (!machine.CheckFollowPlayer())
        {
            if (machine.CheckReachDestination())
            {
                Debug.Log("reached dest");
                machine.ChangeState(machine.idleState);
            }
        }
        else
        {
            if(machine.CheckReachDestination())
                machine.ChangeState(machine.followState);
        }

        if (machine.RetriggerConversation())
            machine.ChangeState(machine.talkState);

    }

    protected override void OnExit()
    {
        //machine.ResetAnimTrigger("Move");
        //machine.StopNavigation();
    }
}
