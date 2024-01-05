using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Zayne : NPCControl
{
    protected override void Start()
    {
        base.Start();
        talkable = true;
    }

    public void ZayneAction1()
    {
        noTalkInWalk = false;
        noLookInConvo = true;
    }

    public void ZayneAction2()
    {
        noTalkInWalk = false;
        noTalkStage = true;
    }


    protected override void OnConversationEnd(Transform other)
    {
        inConversation = false;
        firstTalked = true;
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        currentDialogue.gameObject.SetActive(false);
        noTalkInWalk = true;
    }

    public void ChangePizzaDialogue()
    {
        StopIdle();
        noTalkStage = false;
        firstTalked = false;
        currentDialogue = dialogueHolder.GetChild(2);
        SetMainTalkFalse();
        string reTriggerName = "NPC/" + gameObject.name + "/Other_Interacted";
        DialogueLua.SetVariable(reTriggerName, false);
        noMoveAfterTalk = true;
    }


}
