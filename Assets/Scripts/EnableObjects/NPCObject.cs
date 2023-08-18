using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Cinemachine;

public class NPCObject : LivableObject
{
    public BoxCollider convoTrigger;

    [Header("Object Activation")]
    public bool objectOriented;
    public LivableObject npcObject;


    [Header("Looking Activation")]
    public bool lookingOriented;
    public LookingObject npcLooking;

    DialogueSystemTrigger dialogue;
    public CinemachineVirtualCamera NPCCinemachine;
    public CinemachineVirtualCamera playerCinemachine;
    FixedCameraObject cameraControl;
    public bool firstTalk;
    public bool dialogueEnabled;

    protected override void Start()
    {
        base.Start();
        if (lookingOriented)
            npcLooking = GetComponent<LookingObject>();
        dialogue = GetComponent<DialogueSystemTrigger>();
        cameraControl = GetComponent<FixedCameraObject>();
    }


    protected override void Update()
    {
        base.Update();
        if (npcObject.activated)
        {
            activated = true;
        }

        if (lookingOriented)
        {
            if (npcLooking.activated)
                activated = true;
        }

        if (activated && !firstTalk)
        {
            StartConversation();
        }

        if(firstTalk && interactable)
        {
            gameObject.layer = 9;
            if (Input.GetMouseButtonDown(0))
            {
                StartConversation();
            }
        }
        else
        {
            gameObject.layer = 0;
        }

        if (dialogueEnabled)
        {
            GameObject.Find("PlayerObj").GetComponent<Renderer>().enabled = false;
        }
        else
        {
            GameObject.Find("PlayerObj").GetComponent<Renderer>().enabled = true;
        }

    }

    void StartConversation()
    {
        dialogue.enabled = true;
        dialogueEnabled = true;
        firstTalk = true;
    }

    void OnConversationStart(Transform other)
    {
        player.GetComponent<PlayerHolding>().inDialogue = true;
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    void OnConversationEnd(Transform other)
    {
        dialogue.enabled = false;
        player.GetComponent<PlayerHolding>().inDialogue = false;
        player.GetComponent<PlayerMovement>().enabled = true;
    }





    public void ActivateConvoTrigger()
    {
        convoTrigger.enabled = true;
    }

    public void DeactivateConvoTrigger()
    {
        convoTrigger.enabled = false;
    }
}
