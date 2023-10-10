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
        firstTalk = true;

    }

    protected override void Update()
    {
        base.Update();
        if (triggerObject.activated)
        {
            DialogueLua.SetVariable("NPC_Felix_ActivateType", "CanActivate");
        }
    }
    public void FelixAction1()
    {
        noLookInConvo = true;
        currentDialogue = dialogues[2];

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

    protected override void QuestAcceptChange()
    {
        currentDialogue = dialogues[1];
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
