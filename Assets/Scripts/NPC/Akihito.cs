    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using VInspector;

public class Akihito : NPCControl 
{
    [Foldout("Progression Refs")]
    public Transform akiPizza;
    public FixedCameraObject izaChair;
    public Door bathroomDoor;
    public LocationController izaZone;
    public bool akiToPizza;
    public bool akiPizzaAte;

    [Foldout("Special Dialogues")]
    public Transform akiConfrontation;


    protected override void Update()
    {
        base.Update();
        if (akiToPizza)
            MoveAkiToPizza();

        if(akiPizzaAte && !izaZone.inZone)
        {
            akiPizzaAte = false;
            talkable = false;
            StopIdle();

        }


    }


    public void AkihitoAction0()
    {
        talkable = false;
    }

    public void AkihitoAction1()
    {

    }

    public void AkihitoAction2()
    {
        talkable = false;
        currentDialogue.gameObject.SetActive(true);
        akiPizza.gameObject.SetActive(true);
    }

    public void AkihitoAction3() {

        currentDialogue.gameObject.SetActive(true);
        if (bathroomDoor.enabled)
        {
            bathroomDoor.CloseDoor();
            bathroomDoor.enabled = false;
        }

    }

    public void MoveAkiToPizza()
    {
        _counter = 1;
        StopIdle();

    }



    protected override void OnConversationStart(Transform other)
    {
        base.OnConversationStart(other);
    }

    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);
        talkable = false;

        if(_counter == 2)
        {
            MainQuestState.demoProgress++;
        }
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

    public void AkiTalkable()
    {
        talkable = true;
        overrideNoControl = true;
    }



}
