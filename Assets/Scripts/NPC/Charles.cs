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
    public EatObject pizzaSlice;
    public Akihito aki;
    public Door apartDoor;
    public Collider windowStepCollider;
    public CharacterTattoo cigTat;
    public CharacterTattoo pizzaTat;
    public LocationController apartZone;

    bool readyfirstMove;
    bool readyBathroomMove;
    float firstWaitTime = 30f;

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
        if (readyfirstMove && !interactable)
        {
            if (!inConversation)
            {
                if(aki._counter == 3)
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

        if(readyBathroomMove && !apartZone.inZone)
        {
            StopIdle();
        }
    }

    public void CharlesAction2()
    {
        noLookInConvo = true;
        charlesFollowDia.SetActive(false);
        charlesPizza.SetActive(true);
        talkable = false;
        currentDialogue.gameObject.SetActive(true);

        if (inConversation)
        {
            apartDoor.enabled = false;
            windowStepCollider.enabled = false;
        }
    }

    public void CharlesAction3()
    {
        noTalkStage = true;
        if(pizzaSlice != null)
        {
            pizzaSlice.enabled = true;
        }
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
            apartDoor.enabled = true;
            windowStepCollider.enabled = true;
            readyBathroomMove = true;
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
        machine.SetAnimatorTrigger("Finish");
    }


}
