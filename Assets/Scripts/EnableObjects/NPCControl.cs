using NPCFSM;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCControl : MonoBehaviour
{

    private float matColorVal;
    [Header("[Activate Check]")]
    public bool initialActivated;
    public bool npcActivated;
    public bool fakeActivated;
    public bool overrideNoControl;
    [SerializeField] protected float fadeInterval;
    [SerializeField] protected float minDist;
    [SerializeField] protected bool isVisible;
    [SerializeField] protected bool interactable;
    [SerializeField] public bool followingPlayer;
    [SerializeField] public float npcVincinity;


    [Header("References")]
    public Transform npcMesh;
    public RiggedVisibleDetector visibleDetector;


    [Header("Trigger Types")]
    public bool objectTriggered;
    public LivableObject triggerObject;
    public bool peopleTriggered;
    public Transform otherNPC;
    public bool questTriggered;
    public bool questAccepted;


    protected Transform player;
    public DialogueSystemTrigger currentDialogue;
    protected DialogueSystemTrigger previousDialogue;
    protected Animator anim;
    protected NavMeshAgent agent;
    protected BaseStateMachine machine;




    protected bool firstTalk;

    [Header("[Route Control]")]
    public Transform[] destinations;
    public Component[] destObjects;
    public float[] waitTimes;
    public bool[] destSpecialAnim;
    public DialogueSystemTrigger[] dialogues;

    public int _counter = 0;
    public bool finalStop;
    Transform currentStop;

    [Header("[Conversation]")]
    public bool inConversation;
    public bool inCD;
    public bool triggerConversation;
    private float talkCD = 2f;



    [Header("[Idle State]")]
    public float idleTime;
    protected string idleAction;
    public bool idling;
    public bool idlePaused;
    public bool idleComplete;
    
    
    protected Coroutine lookCoroutine;
    bool lookCoroutineRuning;
    protected bool noLookInConvo;
    protected bool noTalkStage;
    PlayerHolding playerHolding;

    GameObject bone;
    
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Setting up basic components
        player = GameObject.Find("Player").transform;
        matColorVal = 1f;
        //if (initialActivated)
        //    npcActivated = true;

        machine = GetComponent<BaseStateMachine>();
        currentDialogue = dialogues[_counter];
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerHolding = player.GetComponent<PlayerHolding>();
        //FindTheChosenOne(transform);
        foreach(Transform child in transform)
        {
            if (child.name == "iconPos")
                bone = child.gameObject;
        }
    }

    //void FindTheChosenOne(Transform t)
    //{
    //    foreach (Transform child in t)
    //    {
    //        if (child.name == "mixamorig:HeadTop_End")
    //            bone = child.gameObject;
    //        else if (child.childCount != 0)
    //            FindTheChosenOne(child);
    //    }
    //}

    protected virtual void Update()
    {
        //Basic visible and interactable detection
        isVisible = visibleDetector.isVisible;
        interactable = CheckInteractable();

        CheckNPCActivation();
        CheckTalkCD();
        CheckIdle();


        if (npcActivated || fakeActivated)
        {
            if(matColorVal > 0f)
            {
                ActivateAll(npcMesh);
                if (!currentDialogue.enabled)
                    currentDialogue.enabled = true;
            }

        }
        else
        {
            if (matColorVal < 1f)
            {
                DeactivateAll(npcMesh);
            }
        }

        


        if(!StartSequence.noControl || overrideNoControl)
            CheckTriggerConversation();


    }



    #region ActivateFunctionality
    void CheckNPCActivation()
    {
        if (objectTriggered)
        {
            if (triggerObject != null && triggerObject.activated)
                npcActivated = true;
        }

        if (peopleTriggered)
        {
            float playerDist = Vector3.Distance(player.position, transform.position);
            float otherNPCDist = Vector3.Distance(otherNPC.position, transform.position);

            if (playerDist < npcVincinity && otherNPCDist < npcVincinity)
                npcActivated = true;
        }
    }

    bool CheckInteractable()
    {
        if (Vector3.Distance(transform.position, player.position) <= minDist)
        {
            if (isVisible)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }


    public void ActivateAll(Transform obj)
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

    public void DeactivateAll(Transform obj)
    {
        if (obj.GetComponent<Renderer>() != null)
        {
            Material mat = obj.GetComponent<Renderer>().material;
            if (mat.HasProperty("_WhiteDegree"))
            {
                mat.EnableKeyword("_WhiteDegree");
                if (mat.GetFloat("_WhiteDegree") <= 1)
                    TurnOffColor(mat);
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
                    if (childMat.GetFloat("_WhiteDegree") <= 1)
                        TurnOffColor(childMat);
                }

            }
            else
            {
                DeactivateAll(child);
            }
        }
    }

    protected virtual void TurnOnColor(Material material)
    {
        if (matColorVal > 0)
        {
            matColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            material.SetFloat("_WhiteDegree", matColorVal);
        }
        else
        {
            matColorVal = 0;
        }
    }

    protected virtual void TurnOffColor(Material material)
    {
        if (matColorVal < 1)
        {
            matColorVal += 0.1f * fadeInterval * Time.deltaTime;
            material.SetFloat("_WhiteDegree", matColorVal);
        }
        else
        {
            matColorVal = 1;
        }
    }

    public void ActivateDesignatedObject()
    {
        if (Vector3.Distance(transform.position, player.position) < npcVincinity)
        {
            if (destObjects[_counter-1] != null)
            {
                if (destObjects[_counter - 1].transform.GetComponent<LivableObject>())
                    destObjects[_counter - 1].transform.GetComponent<LivableObject>().activated = true;
                if (destObjects[_counter - 1].transform.GetComponent<BuildingGroupController>())
                    destObjects[_counter - 1].transform.GetComponent<BuildingGroupController>().activateAll = true;
            }

        }
    }

    #endregion

    #region Idle Region
    void CheckIdle()
    {
        if (idling && !idlePaused)
        {
            if (idleTime > 0)
                idleTime -= Time.deltaTime;
            else
            {
                idling = false;
                if(lookCoroutine != null)
                    StopCoroutine(lookCoroutine);
            }
        }
    }

    public void StopIdle()
    {
        idleTime = 0;
        idling = false;
        idlePaused = false;
    }


    public void InvokeIdleFunction()
    {
        Invoke(idleAction, 0);
        ActivateDesignatedObject();
    }

    #endregion


    #region GENERAL
    public State ChooseInitialState(char c)
    {
        switch (c)
        {
            case 'F':
                return machine.frozenState;
            case 'T':
                return machine.talkState;
            case 'M':
                return machine.moveState;
            case 'I':
                return machine.idleState;
            default:
                return machine.frozenState;
        }
    }
    bool iconHidden;
    void ChangeLayer(int layerNumber)
    {
        if (layerNumber == 9)
        {
            playerHolding.talkingTo = bone;
            iconHidden = false;
        }
        else if (layerNumber == 0)
        {
            if (!iconHidden)
            {
                playerHolding.talkingTo = null;
                iconHidden = true;
            }
        }
        foreach (Transform child in npcMesh)
        {
            child.gameObject.layer = layerNumber;
        }
    }

    public IEnumerator RotateTowards(Transform target)
    {
        lookCoroutineRuning = true;
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * 0.1f;
            yield return null;
        }
        lookCoroutineRuning = false;
    }
    #endregion


    #region Conversation Control

    void CheckTriggerConversation()
    {
        string reTriggerName = "NPC_" + gameObject.name + "_Other_Interacted";

        //in distance with player and player not already in conversation
        if (interactable && !playerHolding.inDialogue)
        {
            //check npc conditions to trigger conversation
            if (firstTalk && !noTalkStage && !inCD && !inConversation)
            {
                //if npc in idle states are already talked once 
                if (!DialogueLua.GetVariable(reTriggerName).asBool)
                {
                    //move player to interactable layer
                    ChangeLayer(9);

                    //check player interaction command
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!questTriggered)
                        {
                            if (!npcActivated)
                                npcActivated = true;
                        }
                        else if (questAccepted)
                        {
                            if (!npcActivated)
                                npcActivated = true;
                        }

                        StartTalking();
                    }
                }
            }
            else
                ChangeLayer(0);
        }
        else
            ChangeLayer(0);
    }

    void StartTalking()
    {
        currentDialogue.enabled = true;
    }
    void CheckTalkCD()
    {
        if (inCD && talkCD > 0)
        {
            talkCD -= Time.deltaTime;
        }
        else
        {
            inCD = false;
            talkCD = 2f;
        }
    }

    public bool CheckFirstTalk()
    {
        return firstTalk;
    }
    void OnConversationStart(Transform other)
    {
        inConversation = true;
        if (HasReached(agent) && !noLookInConvo)
        {
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            lookCoroutine = StartCoroutine(RotateTowards(player));
        }
        if (!firstTalk)
        {
            firstTalk = true;
        }


    }

    void OnConversationEnd(Transform other)
    {
        inConversation = false;
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        inCD = true;
        currentDialogue.enabled = false;
        fakeActivated = false;
    }

    public void EndConversation()
    {
        inConversation = false;
    }
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains(gameObject.name))
        {

            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            lookCoroutine = StartCoroutine(RotateTowards(destObjects[_counter - 1].transform));
        }

    }

    #region AI Region
    public Transform GetCurrentDestination()
    {
        return destinations[_counter - 1];
    }

    public bool FinalStop()
    {
        if (_counter == destinations.Length)
            return true;
        return false;
    }
    public Transform GetNext()
    {

        var point = destinations[_counter];

        return point;
    }

    public void SetNPCFollow()
    {
        followingPlayer = true;
        agent.stoppingDistance = 4;
    }


    public bool HasReached(NavMeshAgent agent)
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                return true;
            }
        }
        return false;
    }

    public Transform[] GetDestinations()
    {
        return destinations;
    }

    public void SetWaitTime()
    {
        idleTime =  waitTimes[_counter - 1];
    }

    public void SetWaitAction()
    {
        idleAction =  gameObject.name+"Action"+_counter;
    }

    public void SetDialogue()
    {
        //currentDialogue = dialogues[_counter];
        string reTriggerName = "NPC/" + gameObject.name + "/Other_Interacted";
        DialogueLua.SetVariable(reTriggerName, false);

    }

    public void SetMainTalkeTrue()
    {
        string seTalkName = "NPC/" + gameObject.name + "/Main_Talked";
        DialogueLua.SetVariable(seTalkName, true);
    }

    public void SetMainTalkFalse() {
        string seTalkName = "NPC/" + gameObject.name + "/Main_Talked";
        DialogueLua.SetVariable(seTalkName, false);
    }

    public bool GetSpecialIdleAnim()
    {
        return destSpecialAnim[_counter - 1];
    }


    #endregion

}
