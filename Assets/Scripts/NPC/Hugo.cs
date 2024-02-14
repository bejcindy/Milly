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

    [Foldout("TrashDialogues")]
    public GameObject drinkTalk;
    public GameObject foodTalk;
    public GameObject cigTalk;
    public GameObject trashTalk;
    protected override void Start()
    {
        base.Start();

        noMoveAfterTalk = true;
        noLookInConvo = true;
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

        if(totalFloorCleaned == totalFloorCount)
        {
            if(_counter == 4)
            {
                StopIdle();
            }
        }
    }

    public void HugoAction1()
    {

        currentDialogue.gameObject.SetActive(true);
    }

    public void HugoAction2() { }

    public void HugoAction3()
    {
        if (inConversation)
        {
            npcActivated = true;
        }
        scoreBoard.enabled = true;
        noMoveAfterTalk = false;
    }

    public void HugoAction4()
    {
        talkable = true;
    }

    public void HugoAction5()
    {
        noMoveAfterTalk = true;
        noLookInConvo = true;
    }

    public void TriggerWillowTattoo()
    {
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

    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);

        if(_counter == 3)
        {
            ActivateTattooMenu();
        }
        if(_counter == 4)
        {
            TriggerWillowTattoo();
        }
    }




}
