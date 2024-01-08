using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using VInspector;

public class Zayne : NPCControl
{

    [Foldout("Cutscene")]

    protected override void Start()
    {
        base.Start();
        talkable = true;
    }

    protected override void Update()
    {
        base.Update();

    }
    public void ZayneAction1()
    {
        noTalkInWalk = false;
        noTalkStage = true;
        currentDialogue.gameObject.SetActive(true);
        noMoveAfterTalk = true;
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



    public void ChangeMainQuestDialogue()
    {
        //StopIdle();
        noLookInConvo = true;
        noTalkStage = false;
        firstTalked = false;
        currentDialogue = dialogueHolder.GetChild(2);
        SetMainTalkFalse();
        string reTriggerName = "NPC/" + gameObject.name + "/Other_Interacted";
        DialogueLua.SetVariable(reTriggerName, false);
        noMoveAfterTalk = false;
    }

    public void ChangePizzaDialogue()
    {
        StopIdle();
        noTalkStage = false;
        firstTalked = false;
        currentDialogue = dialogueHolder.GetChild(3);
        SetMainTalkFalse();
        string reTriggerName = "NPC/" + gameObject.name + "/Other_Interacted";
        DialogueLua.SetVariable(reTriggerName, false);
        noMoveAfterTalk = true;
    }


    public void MoveZayneAfterWindow()
    {
        noMoveAfterTalk = false;
        StopIdle();
    }



}
