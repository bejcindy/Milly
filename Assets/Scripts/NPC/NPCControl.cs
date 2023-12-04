using NPCFSM;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;
using UnityEngine.Events;

public class NPCControl : MonoBehaviour
{
    protected float matColorVal;
    [SerializeField] protected float minDist;
    [SerializeField] protected bool isVisible;
    [SerializeField] protected bool interactable;
    [SerializeField] public bool followingPlayer;
    [SerializeField] public float npcVincinity;
    protected Transform player;
    protected Transform currentDialogue;
    protected Animator anim;
    protected NavMeshAgent agent;
    protected BaseStateMachine machine;


    [Header("[Activate Check]")]
    public bool mainNPC;
    public bool inCutscene;
    public bool initialActivated;
    public bool npcActivated;

    public bool hasFakeActivate;
    public bool fakeActivated;
    public bool overrideNoControl;
    protected float fadeInterval;




    [Header("References")]
    public Transform npcMesh;
    public RiggedVisibleDetector visibleDetector;


    [Header("Trigger Types")]
    public bool onHoldChar;
    public bool objectTriggered;
    public LivableObject triggerObject;
    public bool peopleTriggered;
    public Transform otherNPC;
    public bool questTriggered;
    public bool questAccepted;


    [Header("[Route Control]")]
    public Transform[] destinations;
    public Component[] destObjects;
    public bool[] destSpecialAnim;
    protected Transform dialogueHolder;

    public int _counter = 0;
    public bool finalStop;
    Transform currentStop;

    [Header("[Conversation]")]
    public bool talkable;
    public bool firstTalked;
    public bool inConversation;
    private bool inCD;
    private float talkCD = 2f;
    public bool noTalkInWalk;
    public bool noMoveAfterTalk;



    [Header("[Idle State]")]
    protected string idleAction;
    public bool idling;
    public bool stopIdleAfterConvo;


    protected Coroutine lookCoroutine;
    bool lookCoroutineRuning;
    public bool noLookInConvo;
    protected bool noTalkStage;
    PlayerHolding playerHolding;

    GameObject bone;

    [Header("Sound")]
    public EventReference footStepSF;

    public UnityEvent OnActivateEvent;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Setting up basic components
        player = GameObject.Find("Player").transform;
        
        machine = GetComponent<BaseStateMachine>();
        dialogueHolder = transform.GetChild(2);
        currentDialogue = dialogueHolder.GetChild(0);

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        playerHolding = player.GetComponent<PlayerHolding>();

        bone = transform.GetChild(3).gameObject;

        //foreach (Transform child in transform)
        //{
        //    if (child.name == "iconPos")
        //        bone = child.gameObject;
        //}


        fadeInterval = 5;
    }


    protected virtual void Update()
    {
        //Basic visible and interactable detection
        isVisible = visibleDetector.isVisible;
        interactable = CheckInteractable();

        CheckNPCActivation();
        CheckTalkCD();
        //CheckIdle();

        if (!mainNPC)
        {
            if (npcActivated)
            {
                //ChangeLayer(17);
                if (matColorVal > 0f)
                {
                    if (OnActivateEvent != null)
                    {
                        OnActivateEvent.Invoke();
                    }
                    ActivateAll(npcMesh);
                    if (!currentDialogue.gameObject.activeSelf)
                        currentDialogue.gameObject.SetActive(true);
                }

            }

            if (fakeActivated)
            {
                if (matColorVal > 0f)
                {
                    ActivateAll(npcMesh);
                    if (!currentDialogue.gameObject.activeSelf)
                        currentDialogue.gameObject.SetActive(true);
                }
            }
            else if(hasFakeActivate && !fakeActivated && !npcActivated && !initialActivated)
            {
                DeactivateAll(npcMesh);

            }
        }


        if (interactable)
        {
            if ((!StartSequence.noControl || overrideNoControl) && !noTalkInWalk && !playerHolding.inDialogue)
            {
                CheckTriggerConversation();
            }

        }
        else if(!inConversation)
        {
            ChangeLayer(0);
        }
        else if (initialActivated)
        {
            ChangeLayer(17);
        }
        //else
        //{
        //    if (npcActivated || initialActivated || onHoldChar)
        //    {
        //        if(npcMesh.gameObject.layer != 17)
        //            ChangeLayer(17);
        //    }
        //    else
        //    {
        //        if (npcMesh.gameObject.layer != 0)
        //            ChangeLayer(0);
        //    }
        //}




    }

    public void PlayFootstepSF()
    {
        if (!footStepSF.IsNull)
            RuntimeManager.PlayOneShot(footStepSF, transform.position);
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
            if (destObjects[_counter - 1] != null)
            {
                if (destObjects[_counter - 1].transform.GetComponent<LivableObject>())
                    destObjects[_counter - 1].transform.GetComponent<LivableObject>().activated = true;
                if (destObjects[_counter - 1].transform.GetComponent<BuildingGroupController>())
                    destObjects[_counter - 1].transform.GetComponent<BuildingGroupController>().activateAll = true;
            }

        }
    }

    public void ActivateNPC()
    {
        npcActivated = true;
    }

    public void FakeActivateNPC()
    {
        fakeActivated = true;
    }

    #endregion

    #region Idle Region

    public void StopIdle()
    {
        idling = false;
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
    public void ChangeLayer(int layerNumber)
    {
        if (layerNumber == 9)
        {
            playerHolding.talkingTo = bone;
            iconHidden = false;
        }
        else if (layerNumber == 0 || layerNumber == 17)
        {
            if (!iconHidden)
            {
                playerHolding.talkingTo = null;
                iconHidden = true;
            }
        }

        npcMesh.gameObject.layer = layerNumber;
        var children = npcMesh.GetComponentsInChildren<Transform>();
        foreach (var child in children)
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

        //check npc conditions to trigger conversation
        if (talkable && !noTalkStage && !inCD && !inConversation)
        {
            //if npc in idle states are already talked once 
            if (!DialogueLua.GetVariable(reTriggerName).asBool)
            {
                //move player to interactable layer
                ChangeLayer(9);

                //check player interaction command
                if (Input.GetMouseButtonDown(0))
                {
                    ChangeLayer(17);
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
        else if (inConversation)
            ChangeLayer(17);
        
    }

    protected void StartTalking()
    {
        currentDialogue.gameObject.SetActive(true);
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

    public bool CheckTalkable()
    {
        return talkable;
    }

    public bool CheckFirstTalked()
    {
        return firstTalked;
    }

    public void QuestAccept()
    {
        questAccepted = true;
    }

    public bool CheckNoMoveAfterTalk()
    {
        return noMoveAfterTalk;
    }

    protected virtual void QuestAcceptChange()
    {

    }
    protected virtual void OnConversationStart(Transform other)
    {
        inConversation = true;
        ChangeLayer(17);
        if (HasReached(agent) && !noLookInConvo)
        {
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            lookCoroutine = StartCoroutine(RotateTowards(player));
        }
    }

    protected virtual void OnConversationEnd(Transform other)
    {
        inConversation = false;
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        inCD = true;
        currentDialogue.gameObject.SetActive(false);

        fakeActivated = false;
        talkable = true;
        firstTalked = true;

        if (stopIdleAfterConvo)
            StopIdle();
    }

    public void EndConversation()
    {
        inConversation = false;
    }
    #endregion


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.name.Contains(gameObject.name))
    //    {

    //        if (lookCoroutine != null)
    //            StopCoroutine(lookCoroutine);
    //        lookCoroutine = StartCoroutine(RotateTowards(other.transform.GetChild(0).transform));
    //    }

    //}

    public void IdleRotate()
    {
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        lookCoroutine = StartCoroutine(RotateTowards(destinations[_counter-1].transform.GetChild(0).transform));
    }

    public void StopLookRotation()
    {
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
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


    public void SetWaitAction()
    {
        idleAction =  gameObject.name+"Action"+_counter;
    }

    public void SetDialogue()
    {
        int diaIndex = currentDialogue.GetSiblingIndex();
        if(diaIndex != dialogueHolder.childCount - 1)
        {
            currentDialogue = dialogueHolder.GetChild(diaIndex + 1);
            string reTriggerName = "NPC/" + gameObject.name + "/Other_Interacted";
            DialogueLua.SetVariable(reTriggerName, false);
        }


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

    public bool GetMainTalked()
    {
        string seTalkName = "NPC/" + gameObject.name + "/Main_Talked";
        return DialogueLua.GetVariable(seTalkName).asBool;
    }

    public bool GetSpecialIdleAnim()
    {
        if (_counter > 0)
            return destSpecialAnim[_counter - 1];
        else
            return destSpecialAnim[_counter];
    }


    #endregion

}
