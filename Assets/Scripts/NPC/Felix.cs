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
        SetMainTalkFalse();
        currentDialogue = questAcceptDia;

    }

    public void ChangeFelixCompleteDialogue()
    {
        SetMainTalkFalse();
        currentDialogue = questCompleteDia;
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

    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);
        if (!menuFirstTriggered)
        {
            menuFirstTriggered = true;
            ActivateTattooMenu();
        }

        if(_counter == 1)
        {
            MainQuestState.demoProgress++;
        }
    }
}
