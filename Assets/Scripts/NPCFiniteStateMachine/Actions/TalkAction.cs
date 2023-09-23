using System;
using System.Collections.Generic;
using UnityEngine;
using NPCFSM;
using PixelCrushers.DialogueSystem;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "NPCFSM/Action/Talk")]
public class TalkAction : NPCFSMAction
{

    public override void Execute(BaseStateMachine machine)
    {
        var _dialogue = machine.GetComponent<DialogueSystemTrigger>();
        var _animator = machine.GetComponent<Animator>();
        var _navAgent = machine.GetComponent<NavMeshAgent>();
        var _npcControl = machine.GetComponent<NPCControl>();

        _navAgent.isStopped = true;
        if (!_npcControl.inCD)
            _dialogue.enabled = true;


    }
}
