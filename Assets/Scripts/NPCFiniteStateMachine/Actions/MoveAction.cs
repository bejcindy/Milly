using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPCFSM;

[CreateAssetMenu(menuName = "NPCFSM/Action/Move")]

public class MoveAction: NPCFSMAction
{
    public override void Execute(BaseStateMachine machine)
    {
        var navMeshAgent = machine.GetComponent<NavMeshAgent>();
        var movePoints = machine.GetDestinations();
        var npcControl = machine.GetComponent<NPCControl>();
        npcControl.idling = false;
        if (!movePoints.HasReached(navMeshAgent))
        {
            npcControl.idleComplete = false;
            navMeshAgent.isStopped = false;
        }

        else if (movePoints.HasReached(navMeshAgent) && !movePoints.FinalStop())
        {
            //navMeshAgent.SetDestination(movePoints.GetNext().position);
            navMeshAgent.isStopped = true;
        }

        else if(movePoints.HasReached(navMeshAgent) && movePoints.FinalStop())
        {
            navMeshAgent.isStopped = true;
            npcControl.NpcFinished();
        }
    }
}
