using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCFSM;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "NPCFSM/Decisions/Finish Idling")]
public class IdleFinishedDecision : Decision
{
    public override bool Decide(BaseStateMachine machine)
    {
        var _npcControl = machine.GetComponent<NPCControl>();
        var _destinations = machine.GetComponent<NPCDestinations>();
        var _agent = machine.GetComponent<NavMeshAgent>();

        if (_npcControl.idleComplete)
        {
            _npcControl.SetAnimatorTrigger("Move");
            return true;
        }
        return false;
    }
}
