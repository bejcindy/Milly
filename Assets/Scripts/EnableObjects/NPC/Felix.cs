using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Felix : NPCControl
{
     
    public BuildingGroupController hintOne;

    protected override void Start()
    {
        base.Start();
        questTriggered = true;
        talkable = true;
        hasFakeActivate = true;
        noLookInConvo = true;

    }

    protected override void Update()
    {
        base.Update();
    }
    public void FelixAction1()
    {
        noLookInConvo = true;

    }

    public void FelixAction2()
    {
        noTalkStage = true;
    }

    public void FelixStandUp()
    {
        machine.SetAnimatorTrigger("StandUp");
    }

    public void ActivateHintOne()
    {
        hintOne.activateAll = true;
    }

    public void ChangeFelixQuestDialogue()
    {
        currentDialogue = dialogueHolder.GetChild(1);
        machine.SetMainTalkFalse();
        string reTriggerName = "NPC/" + gameObject.name + "/Other_Interacted";
        DialogueLua.SetVariable(reTriggerName, false);
    }

    public void CanActivate()
    {
        if (questAccepted)
        {
            npcActivated = true;
        }
        else
        {
            fakeActivated = true;
        }
    }
}
