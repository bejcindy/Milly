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
    public bool conversationFinished;
    Animator anim;
    Coroutine lookCoroutine;
    public DialogueSystemTrigger throwReaction;

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
            if (GetComponent<GroupMaster>())
            {
                GetComponent<GroupMaster>().activateAll = true;
            }
        }

        if ((firstTalk || talkActivated) && interactable && !inCD && !talking && !conversationFinished)
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
            if (mat.HasProperty("_WhiteDegree"))
            {
                mat.EnableKeyword("_WhiteDegree");
                if (mat.GetFloat("_WhiteDegree") >= 0)
                    TurnOnColor(mat);
            }

        }
        foreach (Transform child in obj)
        {
            if (child.childCount <= 0 && child.GetComponent<Renderer>() != null)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                if (childMat.HasProperty("_WhiteDegree"))
                {
                    childMat.EnableKeyword("_WhiteDegree");
                    if (childMat.GetFloat("_WhiteDegree") >= 0)
                        TurnOnColor(childMat);
                }

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
        lookCoroutine = StartCoroutine(RotateTowardsPlayer());
        playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
        playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
        player.GetComponent<PlayerHolding>().inDialogue = true;
        player.GetComponent<PlayerMovement>().enabled = false;
        if (GetComponent<NPCNavigation>())
            GetComponent<NPCNavigation>().talking = true;
        talking = true;
        anim.ResetTrigger("Move");
        anim.SetTrigger("Stop");
    }

    void OnConversationEnd(Transform other)
    {
        StopCoroutine(lookCoroutine);
        playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 300;
        playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 300;
        dialogue.enabled = false;
        player.GetComponent<PlayerHolding>().inDialogue = false;
        player.GetComponent<PlayerMovement>().enabled = true;

        if (movingNPC)
        {
            anim.ResetTrigger("Stop");
            anim.SetTrigger("Move");
            //Invoke(nameof(StartMoving), 2f);

        }
        talking = false;
        inCD = true;

    }

    public void FinishConversation()
    {
        conversationFinished = true;
    }

    IEnumerator RotateTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * 0.1f;
            yield return null;
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Throwable")
        {
            throwReaction.enabled = true;
            npcActivated = true;
        }
    }

    public void StartMoving()
    {

        if (GetComponent<NPCNavigation>())
        {
            GetComponent<NPCNavigation>().enabled = true;
            GetComponent<NPCNavigation>().talking = false;
        }
    }





}
