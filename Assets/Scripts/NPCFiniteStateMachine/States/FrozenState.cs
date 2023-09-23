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
    }

    protected override void OnExit()
    {
        machine.SetAnimatorTrigger("Start");
        machine.ActivateNPC();
    }
}
