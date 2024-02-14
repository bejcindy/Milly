    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using VInspector;

public class Akihito : NPCControl 
{
    public Transform akiLantern;
    public Door bathroomDoor;
    public bool akiToPizza;
    public bool akiPizzaAte;

    [Foldout("Special Dialogues")]
    public Transform akiConfrontation;

    protected override void Update()
    {
        base.Update();
        if (akiToPizza)
            MoveAkiToPizza();

        if(akiPizzaAte && !interactable)
        {
            akiPizzaAte = false;
            remainInAnim = false;
            talkable = false;
            StopIdle();

        }
    }




    public void AkihitoAction1()
    {
    }

    public void AkihitoAction2()
    {
        talkable = false;
        noMoveAfterTalk = true;
        noLookInConvo = true;
        currentDialogue.gameObject.SetActive(true);
    }

    public void AkihitoAction3() {

        currentDialogue.gameObject.SetActive(true);
    }

    public void MoveAkiToPizza()
    {
        _counter = 1;
        StopIdle();
        remainInAnim = false;
        noMoveAfterTalk = false;

    }



    protected override void OnConversationStart(Transform other)
    {
        base.OnConversationStart(other);
    }

    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);
        talkable = false;

    }

    public void AkiStopLooking()
    {
        allowLookPlayer = false;
    }

    public void AkiStartLooking()
    {
        allowLookPlayer = true;
    }

    public void SetAkiPizzaAte()
    {
        akiPizzaAte = true;
    }


    public void ChangeAkiConfrontationDia()
    {
        talkable = true;
        overrideNoControl = true;
        currentDialogue = akiConfrontation;
        SetMainTalkFalse();
    }

    public void LanternFall()
    {
        akiLantern.GetComponent<HingeJoint>().breakForce = 0;
        akiLantern.GetComponent<HingeJoint>().connectedBody = null;
        akiLantern.GetComponent<CollisionObject>().enabled = false;
        akiLantern.GetComponent<GroupMaster>().enabled = false;
        akiLantern.GetComponent<PickUpObject>().enabled = true;
    }


}
