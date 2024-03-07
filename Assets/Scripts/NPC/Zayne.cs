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

    public void ZayneAction0()
    {

    }

    public void ZayneAction1()
    {
        talkable =false;
        currentDialogue.gameObject.SetActive(true);
    }

    public void ZayneAction2()
    {
        talkable = false;
        gameObject.SetActive(false);
    }


    protected override void OnConversationEnd(Transform other)
    {
        inConversation = false;

        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        currentDialogue.gameObject.SetActive(false);
    }



    public void ChangeMainQuestDialogue()
    {
        talkable = true;
        currentDialogue.gameObject.SetActive(false);
        currentDialogue = dialogueHolder.GetChild(2);
        currentDialogue.gameObject.SetActive(true);
        SetMainTalkFalse();
    }



    public void MoveZayneAfterWindow()
    {
        StopIdle();
    }



}
