using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Cinemachine;

public class NPCObject : LivableObject
{

    public int conversationID;
    public bool npcActivated;
    [Header("Object Activation")]
    public bool objectOriented;
    public LivableObject npcObject;


    [Header("Looking Activation")]
    public bool lookingOriented;
    public LookingObject npcLooking;

    DialogueSystemTrigger dialogue;
    public CinemachineVirtualCamera NPCCinemachine;
    public CinemachineVirtualCamera playerCinemachine;

    public GameObject npcBody;
    public bool firstTalk;
    public bool dialogueEnabled;
    Animator anim;

    protected override void Start()
    {
        base.Start();
        if (lookingOriented)
            npcLooking = GetComponent<LookingObject>();
        dialogue = GetComponent<DialogueSystemTrigger>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }


    protected override void Update()
    {
        base.Update();
        if (npcObject.activated)
        {
            npcActivated = true;
        }

        if (lookingOriented)
        {
            if (npcLooking.activated)
                npcActivated = true;
        }

        if (npcActivated && !firstTalk)
        {
            StartConversation();
        }

        if(firstTalk && interactable)
        {
            ChangeLayer(9);
            if (Input.GetMouseButtonDown(0))
            {
                StartConversation();
            }
        }
        else
        {
            ChangeLayer(0);
        }

        if (dialogueEnabled)
        {
            GameObject.Find("PlayerObj").GetComponent<Renderer>().enabled = false;
        }


        if (npcActivated)
        {
            anim.SetTrigger("Start");
            ActivateAll(this.transform);
        }

        if (DialogueManager.isConversationActive)
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                if(DialogueManager.lastConversationID == conversationID)
                {
                    DialogueManager.StopConversation();
                }
            }
        }

    }

    void ChangeLayer(int layerNumber)
    {
        foreach(Transform child in npcBody.transform)
        {
            child.gameObject.layer = layerNumber;
        }
    }

    void ActivateAll(Transform obj)
    {
        if (obj.GetComponent<Renderer>() != null)
        {
            Material mat = obj.GetComponent<Renderer>().material;
            mat.EnableKeyword("_WhiteDegree");
            if (mat.GetFloat("_WhiteDegree") >= 0)
                TurnOnColor(mat);
        }
        foreach (Transform child in obj)
        {
            if (child.childCount <= 0 && child.GetComponent<Renderer>() != null)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                childMat.EnableKeyword("_WhiteDegree");
                if (childMat.GetFloat("_WhiteDegree") >= 0)
                    TurnOnColor(childMat);
            }
            else
            {
                ActivateAll(child);
            }
        }
    }

    public void FinishingAnim()
    {
        anim.ResetTrigger("Start");
        anim.SetTrigger("Finish");
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





}
