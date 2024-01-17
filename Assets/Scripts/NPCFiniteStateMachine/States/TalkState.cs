using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : State
{
    protected override void OnEnter()
    {        
        if(!machine.CheckNoLookInTalk())
            machine.TurnOnCam();

        if (machine.CheckNavOn())
        {
            if (machine.CheckReachDestination())
            {
                if (machine.CheckTalkable() && !machine.CheckSpecialIdleAnim() && machine.CheckFirstTalked())
                {
                    Debug.Log("Should Talk");
                    machine.StopNavigation();
                    if(!!machine.CheckRemainInAnim())
                        machine.SetAnimatorTrigger("Talk");
                }
            }
        }
    }
    protected override void OnUpdate()
    {

        if (!machine.CheckInConversation())
        {
            machine.SetMainTalkTrue();
            machine.TurnOffCam();

            if (!machine.CheckNoMoveAfterTalk())
            {
                machine.StopIdling();
                if (!machine.CheckBrainBlending())
                    machine.DelayMove();
            }
            else
            {
                machine.ChangeState(machine.idleState);
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
        machine.ResetCam();
        //machine.ResetAnimTrigger("Stop");
    }

}
