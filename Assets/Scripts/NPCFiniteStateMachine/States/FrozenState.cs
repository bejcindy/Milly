using NPCFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenState : State
{
    protected override void OnUpdate()
    {
        if (machine.CheckNPCActivation())
        {
            machine.ChangeState(machine.talkState);
        }
        else
        {
            if (machine.CheckInConversation() && !machine.CheckQuestActivated())
            {
                machine.FakeActivateNPC();
            }
            else
            {
                machine.FakeDeactivateNPC();
            }
        }
    }

    protected override void OnExit()
    {
        machine.SetAnimatorTrigger("Start");
        machine.ActivateNPC();
    }
}
