using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        currentDialogue.gameObject.SetActive(false);
        noTalkInWalk = true;
    }

}
