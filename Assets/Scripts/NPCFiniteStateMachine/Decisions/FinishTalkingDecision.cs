using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCFSM;
using PixelCrushers.DialogueSystem;

[CreateAssetMenu(menuName = "NPCFSM/Decisions/Finish Talking")]

public class FinishTalkingDecision : Decision
{
    public override bool Decide(BaseStateMachine machine)
    {
        var dialogue = machine.GetComponent<DialogueSystemTrigger>();

        return (!dialogue.enabled);
    }
}
