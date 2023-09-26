using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : State {


    protected override void OnEnter()
    {

        machine.PauseIdling();
        machine.StartConversation();
        machine.StopNavigation();


    }
    protected override void OnUpdate()
    {

        if (!machine.CheckNPCInDialogue())
        {
            if (!machine.CheckFollowPlayer())
            {
                if (machine.CheckIdleFinished())
                    machine.ChangeState(machine.moveState);
                else
                    machine.ChangeState(machine.idleState);
            }
            else
            {
                machine.ChangeState(machine.moveState);
            }

        }

    }

    protected override void OnExit()
    {
        //machine.ResetAnimTrigger("Stop");
        machine.UnPauseIdling();
    }

}
