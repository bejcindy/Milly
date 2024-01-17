using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using VInspector;

public class Felix : NPCControl
{
     
    public BuildingGroupController hintOne;

    [Foldout("Special Dialogues")]
    public Transform questAcceptDia;
    public Transform questCompleteDia;

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
        currentDialogue = questAcceptDia;
        SetMainTalkFalse();
    }

    public void ChangeFelixCompleteDialogue()
    {
        firstTalked = false;
        currentDialogue = questCompleteDia;
        SetMainTalkFalse();
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
