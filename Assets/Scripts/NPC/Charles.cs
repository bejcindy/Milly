using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using FMODUnity;
using VInspector;

public class Charles : NPCControl
{
    public bool charlesSmoking;
    public bool charlesOpenDoor;

    [Foldout("Progression References")]
    public GameObject charlesPizza;
    public GameObject charlesFollowDia;
    public Ron ron;
    public CharacterTattoo cigTat;
    public CharacterTattoo pizzaTat;

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
        charlesFollowDia.SetActive(false);
        charlesPizza.SetActive(true);
        talkable = false;
        currentDialogue.gameObject.SetActive(true);
    }

    public void CharlesAction3()
    {
        noTalkStage = false;
    }


    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);

        if(_counter == 1)
        {
            cigTat.triggered = true;
        }

        if (_counter == 2)
        {
            pizzaTat.triggered = true;
            MainQuestState.demoProgress++;
        }
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
