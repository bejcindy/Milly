using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPCFSM;

[CreateAssetMenu(menuName = "NPCFSM/Decisions/Move To Idle")]
public class MoveToIdleDecision : Decision
{
    public override bool Decide(BaseStateMachine machine)
    {
        var _navAgent = machine.GetComponent<NavMeshAgent>();
        var _destinations = machine.GetComponent<NPCDestinations>();
        var _npcControl = machine.GetComponent<NPCControl>();
        if (_destinations.HasReached(_navAgent))
        {
            Debug.Log("Checking that we are done moving");
            _npcControl.SetAnimatorTrigger("Stop");
            _navAgent.isStopped = true;
            _npcControl.idleTime = _destinations.GetWaitTime();
            _npcControl.idleAction = _destinations.GetWaitAction();
            _navAgent.SetDestination(_destinations.GetNext().position);
            _npcControl.InvokeIdleFunction();
            return true;
        }
        return false;
    }
}
