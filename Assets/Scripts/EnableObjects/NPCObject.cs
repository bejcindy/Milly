using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Cinemachine;

public class NPCObject : LivableObject
{

    public int conversationID;
    public bool npcActivated;
    public bool noActivation; 
    [Header("Object Activation")]
    public bool objectOriented;
    public LivableObject npcObject;


    [Header("Looking Activation")]
    public bool lookingOriented;
    public LookingObject npcLooking;

    [Header("Trigger Activation")]
    public bool triggerActivated;

    public bool talkActivated;

    DialogueSystemTrigger dialogue;
    public CinemachineVirtualCamera NPCCinemachine;
    public CinemachineVirtualCamera playerCinemachine;

    public GameObject npcBody;
    public bool firstTalk;
    public bool dialogueEnabled;
    public bool movingNPC;

    public float talkCD;
    public bool inCD;
    public bool talking;
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

        if (inCD && talkCD > 0)
        {
            talkCD -= Time.deltaTime;
        }
        else
        {
            inCD = false;
            talkCD = 2f;
        }

        if (objectOriented)
        {
            if (npcObject.activated)
            {
                npcActivated = true;
            }
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

        if ((firstTalk || talkActivated) && interactable && !inCD && !talking)
        {
            ChangeLayer(9);
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                StartConversation();
                if (talkActivated)
                    npcActivated = true;
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
            if(!noActivation)
                ActivateAll(this.transform);
        }

        //if (DialogueManager.isConversationActive)
        //{
        //    if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        //    {
        //        if(DialogueManager.lastConversationID == conversationID)
        //        {
        //            DialogueManager.StopConversation();
        //        }
        //    }
        //}

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
        ChangeLayer(0);
        dialogue.enabled = true;
        dialogueEnabled = true;
        firstTalk = true;
    }

    void OnConversationStart(Transform other)
    {
        player.GetComponent<PlayerHolding>().inDialogue = true;
        player.GetComponent<PlayerMovement>().enabled = false;
        if (GetComponent<NPCNavigation>())
            GetComponent<NPCNavigation>().talking = true;
        talking = true;
    }

    void OnConversationEnd(Transform other)
    {
        dialogue.enabled = false;
        player.GetComponent<PlayerHolding>().inDialogue = false;
        player.GetComponent<PlayerMovement>().enabled = true;
        if (movingNPC)
        {
            Invoke(nameof(StartMoving), 2f);

        }
        talking = false;
        inCD = true;

    }

    void OnTriggerEnter(Collider coll)
    {
        if (triggerActivated)
        {
            if (coll.CompareTag("Player"))
            {
                npcActivated = true;
            }
        }
    }

    void StartMoving()
    {
        anim.SetTrigger("Move");
        if (GetComponent<NPCNavigation>())
        {
            GetComponent<NPCNavigation>().enabled = true;
            GetComponent<NPCNavigation>().talking = false;
        }
    }





}
