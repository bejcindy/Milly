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
        noLookInConvo = true;
    }

    public void GloriaAction3()
    {

        noTalkStage = true;
        currentDialogue.gameObject.SetActive(true);
        noLookInConvo = true;

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
        gloriaHugoDia.gameObject.SetActive(true);
    }


    public void GloriaBathroomMove()
    {
        StopIdle();
        remainInAnim = false;
        noMoveAfterTalk = false;
    }


    public void GloriaFifthConversationActive()
    {
        talkable = true;
        noLookInConvo = false;
        noTalkStage = false;
    }


}
