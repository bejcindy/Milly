using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : State
{
    protected override void OnEnter()
    {        
        if(machine.TurnOnCharCam())
            machine.TurnOnCam();

        if(machine.CheckTalkRotate())
            machine.TalkRotate();
        machine.PlayTalkAnimation();
    }
    protected override void OnUpdate()
    {

        if (!machine.CheckInConversation())
        {
            machine.SetMainTalkTrue();
            machine.TurnOffCam();

            machine.ChangeState(machine.idleState);
        }
    }


    protected override void OnExit()
    {
        if(!ReferenceTool.playerBrain.IsBlending)
            machine.ResetCam();
    }

}
