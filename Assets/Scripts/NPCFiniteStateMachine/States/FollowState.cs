using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : State
{

    protected override void OnEnter()
    {
        machine.StopIdling();
        machine.BeginNavigation();
    }

    protected override void OnUpdate()
    {
        if(machine.CheckFollowPlayer())
            machine.FollowPlayer();
    }


    protected override void OnExit()
    {
        base.OnExit();
    }


}
