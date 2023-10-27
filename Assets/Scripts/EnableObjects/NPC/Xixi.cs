using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Xixi : NPCControl
{
    public EventReference catMeowSF;
    protected override void Start()
    {
        base.Start();
        firstTalk = true;
    }

    public void XixiAction1()
    {
        noTalkStage = true;
        noTalkInWalk = false;
    }

    public void XixiAction2()
    {
        noTalkStage = false;
        noTalkInWalk = false;
    }

    public void XixiAction3()
    {
        noTalkStage = true;
        noTalkInWalk = false;
    }
    public void Meow()
    {
        if (!catMeowSF.IsNull)
            RuntimeManager.PlayOneShot(catMeowSF, transform.position);
    }
    void OnConversationEnd(Transform other)
    {
        inConversation = false;
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        currentDialogue.gameObject.SetActive(false);
        noTalkInWalk = true;
    }

    public void ChangeIntialActivate()
    {
        initialActivated = true;
        ChangeLayer(17);
    }


}
