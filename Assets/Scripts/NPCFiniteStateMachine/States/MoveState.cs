using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected override void OnEnter()
    {
        machine.SetNPCDestination();
        machine.BeginNavigation();
        machine.SetAnimatorTrigger("Move");
    }

    protected override void OnUpdate()
    {
        if (machine.CheckReachDestination())
        {
            machine.ChangeState(machine.idleState);
        }
    }

    protected override void OnExit()
    {
        machine.ResetAnimTrigger("Move");
        machine.StopNavigation();
    }
}
