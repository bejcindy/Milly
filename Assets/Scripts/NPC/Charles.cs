using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using FMODUnity;

public class Charles : NPCControl
{
    public bool charlesSmoking;
    public bool charlesOpenDoor;

    public GameObject charlesPizza;
    public GameObject charlesFollowDia;
    public Ron ron;

    bool readyfirstMove;
    float firstWaitTime = 15f;

    protected override void Start()
    {
        base.Start();

    }
    protected override void Update()
    {
        base.Update();
    }

    public void CharlesAction1()
    {

        talkable = true;
        remainInAnim = true;
        noMoveAfterTalk = true;

        if (inConversation)
        {
            readyfirstMove = true;
        }
        if (readyfirstMove)
        {
            if (!inConversation)
            {
                if(ron._counter == 2)
                {
                    if (firstWaitTime > 0)
                        firstWaitTime -= Time.deltaTime;
                    else
                    {
                        StopIdle();
                        charlesFollowDia.SetActive(true);
                    }

                }
            }
        }
    }

    public void CharlesAction2()
    {
        charlesPizza.SetActive(true);
        currentDialogue.gameObject.SetActive(true);
    }

    public void CharlesAction3()
    {
        noTalkStage = false;
    }

    public void MoveCharlesUpstairs()
    {
        string seTalkName = "NPC/" + gameObject.name + "/Main_Talked";
        if (DialogueLua.GetVariable(seTalkName).asBool)
        {
            StopIdle();
            charlesFollowDia.SetActive(true);
        }
    }

    public void FinishSmoking()
    {
        Debug.Log("finishing smoking");
        //machine.ResetAnimTrigger("Special1");
        machine.SetAnimatorTrigger("Finish");
    }


}
