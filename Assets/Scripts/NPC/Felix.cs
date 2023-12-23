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
        noMoveAfterTalk = true;
    }

    public void FelixAction2()
    {
        noTalkStage = true;
        gameObject.SetActive(false);
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
        firstTalked = false;
        currentDialogue = dialogueHolder.GetChild(1);
        SetMainTalkFalse();
        string reTriggerName = "NPC/" + gameObject.name + "/Other_Interacted";
        DialogueLua.SetVariable(reTriggerName, false);
    }

    public void ChangeFelixCompleteDialogue()
    {
        firstTalked = false;
        currentDialogue = dialogueHolder.GetChild(3);
        SetMainTalkFalse();
        string reTriggerName = "NPC/" + gameObject.name + "/Other_Interacted";
        DialogueLua.SetVariable(reTriggerName, false);
        noMoveAfterTalk = false;
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
