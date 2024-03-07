using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloria : NPCControl
{
    public FixedCameraObject chair;

    public Transform gloriaHugoDia;

    bool gloriaOpenDoor;
    bool gloriaIzaConfront;
    protected override void Start()
    {
        base.Start();
        overrideNoControl = true;
        talkable = false;
    }

    public void GloriaAction1()
    {
        talkable = true;
    }
     
    public void GloriaAction2()
    {

    }

    public void GloriaAction3()
    {

        talkable = false;
        currentDialogue.gameObject.SetActive(true);


    }

    public void GloriaAction4()
    {
        talkable = false;
        if (chair.positionFixed && !gloriaIzaConfront)
        {
            gloriaIzaConfront = true;
            currentDialogue.gameObject.SetActive((true));
        }

    }

    public void GloriaAction5()
    {

    }

    public void GloriaAction6()
    {
        talkable = true;
    }

    public void GloriaAction7()
    {
        talkable = false;
    }


    public void GloriaBathroomMove()
    {
        StopIdle();

    }


    public void GloriaFifthConversationActive()
    {
        talkable = true;
    }

    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);
        if(_counter == 6)
        {
            DialogueManager.ShowAlert("You have completed all Main Story related content in the current demo. \n Feel free to continue exploring the environment. \n Thank you for playing Inkression!");
        }
    }


}
