using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCFSM;
using PixelCrushers.DialogueSystem;

[CreateAssetMenu(menuName = "NPCFSM/Decisions/Interacted By Player")]
public class InteractedByPlayerDecision : Decision
{

   public override bool Decide(BaseStateMachine machine)
    {
        var npcControl = machine.GetComponent<NPCControl>();
        var dialogue = machine.GetComponent<DialogueSystemTrigger>();
        
        return (npcControl.reTriggerConversation);
    }
}
