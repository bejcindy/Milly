using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class Hugo : NPCControl 
{
    public static bool trashThrown;
    public static PickUpObject thrownObject;
    public static int totalFloorCount;
    public static int totalFloorCleaned;
    public int floorCount;
    public int floorCleaned;
    public GameObject broom;
    public Transform broomPlacePos;
    public TrashSoccerScoreBoard scoreBoard;

    public GameObject cigButtConvo;
    public CharacterTattoo willowTat;
    bool willowTatTriggered;
    public bool finalWait;

    public float finalWaitTime = 60;

    [Foldout("TrashDialogues")]
    public GameObject drinkTalk;
    public GameObject foodTalk;
    public GameObject cigTalk;
    public GameObject trashTalk;
    bool triggerBroomTat;
    bool broomDiaDone;
    protected override void Start()
    {
        base.Start();

        noMoveAfterTalk = true;
        noCameraLock = true;
        willowTatTriggered = false;
    }

    protected override void Update()
    {
        base.Update();
        floorCleaned = totalFloorCleaned;
        floorCount = totalFloorCount;
        if (trashThrown)
        {
            HugoTrashDetection();
        }

        if(totalFloorCleaned > totalFloorCount *2 / 3 && !broomDiaDone)
        {
            DialogueManager.StartConversation("Thoughts/FloorCleaned");

        }


        if (_counter == 3 && willowTatTriggered && broomDiaDone)
        {
            finalWait = true;
        }

        if (finalWait && !interactable)
        {
            if(finalWaitTime > 0)
            {
                finalWaitTime -= Time.deltaTime;
            }
            else
            {
                finalWaitTime = 0;
                finalWait = false;
                StopIdle();

            }
        }
    }

    public void HugoAction1()
    {

    }

    public void HugoAction2() {
        scoreBoard.enabled = true;
        if (scoreBoard.activated)
        {
            currentDialogue.gameObject.SetActive(true);
        }
        if (inConversation)
        {
            npcActivated = true;
        }
        noMoveAfterTalk = false;
    }

    public void HugoAction3()
    {
        talkable = true;
        noMoveAfterTalk = true;
    }

    public void HugoAction4()
    {
        noMoveAfterTalk = true;
        noCameraLock = true;
        talkable = true;
        noTalkStage = false;
    }



    public void TriggerWillowTattoo()
    {
        willowTatTriggered = true;
        willowTat.triggered = true;
    }

    public void HugoDropBroom()
    {
        broom.transform.SetParent(null);
        broom.transform.position = broomPlacePos.position + new Vector3(0, 1, 0);
        broom.GetComponent<Rigidbody>().isKinematic = false;
        broom.transform.rotation = Quaternion.identity;
    }

    void HugoTrashDetection()
    {
        bool talkVar = DialogueLua.GetVariable("NPC/Hugo/ThrowTalk").asBool;
        DialogueLua.SetVariable("NPC/Hugo/ThrowTalk", !talkVar);
        if(Vector3.Distance(transform.position, thrownObject.transform.position) <= 5)
        {
            switch (thrownObject.objType)
            {
                case HandObjectType.DRINK:
                    drinkTalk.SetActive(true);
                    break;
                case HandObjectType.TRASH:
                    trashTalk.SetActive(true);
                    break;
                case HandObjectType.FOOD:
                    foodTalk.SetActive(true);
                    break;
                case HandObjectType.CIGARETTE:
                    cigButtConvo.SetActive(true);
                    break;
                default:
                    trashTalk.SetActive(true);
                    break;
            }
            trashThrown = false;
            thrownObject = null;
        }
        else
        {
            trashThrown = false;
            thrownObject = null;
        }
    }

    public void SetBroomDiaDone()
    { 
        broomDiaDone = true;
    }

    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);

        if(_counter == 2)
        {
            ActivateTattooMenu();
        }
        if(_counter == 3)
        {
            TriggerWillowTattoo();

        }

        if(_counter == 4)
        {
            MainQuestState.demoProgress++;
        }

    }




}
