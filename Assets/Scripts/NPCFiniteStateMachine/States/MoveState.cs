using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected override void OnEnter()
    {
        if (!machine.CheckFollowPlayer())
            machine.SetNPCDestination();
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
                machine.ChangeState(machine.idleState);
            }
        }
        else
        {
            machine.FollowPlayer();
        }

    }

    protected override void OnExit()
    {
        machine.ResetAnimTrigger("Move");
        machine.StopNavigation();
    }
}
