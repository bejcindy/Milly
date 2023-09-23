using NPCFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[CreateAssetMenu(menuName = "NPCFSM/Action/Frozen")]
public class FrozenAction : NPCFSMAction
{
    public override void Execute(BaseStateMachine machine)
    {
        var _agent = machine.GetComponent<NavMeshAgent>();
        _agent.isStopped = true;
    }
}
