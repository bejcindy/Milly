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
    public Transform boxDialogue;

    protected override void Start()
    {
        base.Start();
        questTriggered = true;
        talkable = true;
        hasFakeActivate = true;
    }

    protected override void Update()
    {
        base.Update();  
    }
    public void FelixAction1()
    {
    }

    public void FelixAction2()
    {
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
        if(!StartSequence.noControl)
        {
            if (!menuFirstTriggered)
            {
                menuFirstTriggered = true;
                if(!myTatMenu.discovered)
                    ActivateTattooMenu();
            }
        }

        if(_counter == 0)
        {
            if(currentDialogue == questAcceptDia)
            {
                StopIdle();
            }
        }

        if(_counter == 1)
        {
            MainQuestState.demoProgress++;
           
        }
    }
}
