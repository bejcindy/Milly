using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    bool isFollowing;
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
        {
            machine.FollowPlayer();
            isFollowing = true;
        }

        machine.BeginNavigation();
        machine.SetAnimatorTrigger("Move");
    }

    protected override void OnUpdate()
    {
        if (!machine.CheckFollowPlayer())
        {
            if (!isFollowing)
            {
                if (machine.CheckReachDestination())
                {
                    Debug.Log("reached dest");
                    machine.SetMainTalkFalse();
                    machine.ChangeState(machine.idleState);
                }
            }
            else
            {
                isFollowing = false;
                machine.SetNPCDestination();
            }

        }
        else
        {
            isFollowing = true;
            if(machine.CheckReachDestination())
                machine.ChangeState(machine.followState);
        }

        if (machine.CheckInConversation())
            machine.ChangeState(machine.talkState);

    }

    protected override void OnExit()
    {
        //machine.ResetAnimTrigger("Move");
        //machine.StopNavigation();
    }
}
