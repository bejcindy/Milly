using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Akihito : NPCControl 
{
    public Transform akiLantern;

    public void ActivateAki()
    {
        ChangeLayer(17);
    }

    public void AkihitoAction1()
    {
        noMoveAfterTalk = true;
        noTalkInWalk = true;
    }

    public void LanternFall()
    {
        akiLantern.GetComponent<HingeJoint>().breakForce = 0;
        akiLantern.GetComponent<HingeJoint>().connectedBody = null;
        akiLantern.GetComponent<CollisionObject>().enabled = false;
        akiLantern.GetComponent<GroupMaster>().enabled = false;
        akiLantern.GetComponent<PickUpObject>().enabled = true;
    }

    protected override void OnConversationStart(Transform other)
    {
        base.OnConversationStart(other);
        anim.SetTrigger("Talk");
    }

    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);
        anim.SetTrigger("Stop");
    }

    public void AkiStopLooking()
    {
        allowLookPlayer = false;
    }

    public void AkiStartLooking()
    {
        allowLookPlayer = true;
    }


    public void SetAkiActive()
    {
        talkable = true;
        overrideNoControl = true;
        firstTalked = false;
        noLookInConvo = true;
        currentDialogue = dialogueHolder.GetChild(1);
        SetMainTalkFalse();
        string reTriggerName = "NPC/" + gameObject.name + "/Other_Interacted";
        DialogueLua.SetVariable(reTriggerName, false);
    }


}
