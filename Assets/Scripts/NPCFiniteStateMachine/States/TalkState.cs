using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : State {


    protected override void OnEnter()
    {

        //Pause Idle first if in idle
        machine.PauseIdling();

        //commence normal behaviors
        machine.StartConversation();
        machine.StopNavigation();
        machine.SetAnimatorTrigger("Stop");
        machine.RotateNPC(machine.player);
    }
    protected override void OnUpdate()
    {
        if (!machine.CheckNPCInDialogue())
        {
            if (machine.CheckIdleFinished())
                machine.ChangeState(machine.moveState);
            else
                machine.ChangeState(machine.idleState);
        }

    }

    protected override void OnExit()
    {
        machine.ResetAnimTrigger("Stop");
        machine.EndConversation();
        machine.UnPauseIdling();
    }

}
