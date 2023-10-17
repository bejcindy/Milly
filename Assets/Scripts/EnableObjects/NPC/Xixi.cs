using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xixi : NPCControl
{
    protected override void Start()
    {
        base.Start();
        firstTalk = true;
        noTalkInWalk = true;
    }

    public void XixiAction1()
    {
        noTalkStage = true;
    }

    public void XixiAction2()
    {
        noTalkStage = false;
    }

    public void XixiAction3()
    {
        noTalkStage = true;
    }

    void OnConversationEnd(Transform other)
    {
        inConversation = false;
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        currentDialogue.gameObject.SetActive(false);
        StopIdle();
    }

}
