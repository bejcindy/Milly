using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloria : NPCControl
{
    float waitTime = 3f;
    public FixedCameraObject chair;

    public Transform gloriaHugoDia;

    bool gloriaOpenDoor;
    bool gloriaIzaConfront;
    protected override void Start()
    {
        base.Start();
        overrideNoControl = true;
        talkable = true;
    }

    public void GloriaAction1()
    {

    }
     
    public void GloriaAction2()
    {
        noCameraLock = true;
    }

    public void GloriaAction3()
    {

        noTalkStage = true;
        currentDialogue.gameObject.SetActive(true);
        noCameraLock = true;

    }

    public void GloriaAction4()
    {
        noMoveAfterTalk = true;
        remainInAnim = true;
        if (chair.positionFixed && !gloriaIzaConfront)
        {
            gloriaIzaConfront = true;
            currentDialogue.gameObject.SetActive((true));
        }

    }

    public void GloriaAction5()
    {
        remainInAnim = false;
        talkable = false;
    }

    public void GloriaAction6()
    {
        noMoveAfterTalk = false;
        talkable = true;
        noTalkStage = false;
    }


    public void GloriaBathroomMove()
    {
        StopIdle();
        remainInAnim = false;
        noMoveAfterTalk = false;
        noCameraLock = false;
    }


    public void GloriaFifthConversationActive()
    {
        talkable = true;
        noCameraLock = false;
        noTalkStage = false;
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
