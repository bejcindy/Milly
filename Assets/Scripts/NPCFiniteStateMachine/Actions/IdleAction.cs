using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCFSM;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "NPCFSM/Action/Idle")]
public class IdleAction : NPCFSMAction
{

    public override void Execute(BaseStateMachine machine)
    {
        var _control = machine.GetComponent<NPCControl>();
        var _movePoints = machine.GetComponent<NPCDestinations>();
        var _agent = machine.GetComponent<NavMeshAgent>();

        Debug.Log("finishing idle");
        _control.idling = true;
        _control.SetAnimatorTrigger("Stop");



            
    }
}
