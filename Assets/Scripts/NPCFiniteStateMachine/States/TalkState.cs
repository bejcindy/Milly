using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : State {


    protected override void OnEnter()
    {

        machine.PauseIdling();
        machine.StartConversation();
        if (machine.CheckReachDestination() && machine.CheckFirstTalk())
        {
            machine.StopNavigation();
            machine.SetAnimatorTrigger("Stop");
        }



    }
    protected override void OnUpdate()
    {

        if (!machine.CheckNPCInDialogue())
        {
            //NOT FOLLOWING NPC AFTER
            if (!machine.CheckFollowPlayer())
            {
                if (machine.CheckIdleFinished())
                    machine.ChangeState(machine.moveState);
                else
                    machine.ChangeState(machine.idleState);
            }
            //FOLLOWING NPC AFTER
            else
            {
                machine.StopIdling();
                if (!machine.CheckPlayerInVincinity())
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
