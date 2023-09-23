using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCFSM;

[CreateAssetMenu(menuName = "NPCFSM/Decisions/First Activation")]
public class InitialActivateDecision : Decision
{
    public override bool Decide(BaseStateMachine machine)
    {
        var _npcControl = machine.GetComponent<NPCControl>();

        return _npcControl.npcActivated && !_npcControl.firstTalk;
    }
}
