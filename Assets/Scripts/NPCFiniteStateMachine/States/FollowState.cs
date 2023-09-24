using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : State
{

    protected override void OnEnter()
    {
        machine.StopIdling();
        machine.BeginNavigation();
        machine.SetAnimatorTrigger("Move");
    }

    protected override void OnUpdate()
    {
        if (machine.CheckFollowPlayer())
        {
            machine.FollowPlayer();
            if (machine.CheckReachDestination())
            {
                machine.ResetAnimTrigger("Move");
                machine.SetAnimatorTrigger("Stop");
            }
            else
            {
                machine.ResetAnimTrigger("Stop");
                machine.SetAnimatorTrigger("Move");
            }
        }

        else
        {
            if (!machine.CheckPathFinished())
                machine.ChangeState(machine.moveState);
        }
            
    }


    protected override void OnExit()
    {
        machine.ResetAnimTrigger("Move");
    }


}
