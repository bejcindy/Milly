using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : State {


    protected override void OnEnter()
    {
        machine.TurnOnCam();
        machine.PauseIdling();
        if (machine.CheckReachDestination())
        {
            if (machine.CheckTalkable() && !machine.CheckSpecialIdleAnim() && machine.CheckFirstTalked())
            {
                machine.StopNavigation();
                machine.SetAnimatorTrigger("Stop");

            }

        }



    }
    protected override void OnUpdate()
    {

        if (!machine.CheckInConversation())
        {
            machine.TurnOffCam();
            //NOT FOLLOWING NPC AFTER
            if (!machine.CheckFollowPlayer())
            {
                if (machine.CheckIdleFinished())
                {
                    if (!machine.CheckBrainBlending())
                        machine.DelayMove();
                }

                else
                {
                    machine.ChangeState(machine.idleState);
                }

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

    public void ChangeToMove()
    {
        machine.ChangeState(machine.moveState);
    }

    protected override void OnExit()
    {
        machine.TurnOffCam();
        //machine.ResetAnimTrigger("Stop");

    }

}
