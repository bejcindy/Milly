using NPCFSM;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;
using UnityEngine.Events;
using VInspector;
using System.Linq;

using Unity.VisualScripting;
using System;

public class NPCControl : MonoBehaviour
{
    PlayerHolding playerHolding;
    protected Transform player;
    protected Animator anim;
    protected NavMeshAgent agent;
    protected BaseStateMachine machine;
    protected Rigidbody rb;



    [Foldout("Default")]

    public float matColorVal;
    [SerializeField] protected float fadeInterval;
    [SerializeField] protected float minDist;
    [SerializeField] protected bool isVisible;
    [SerializeField] protected bool interactable;
    [SerializeField] public float npcVincinity;
    public bool finalStop;

    protected bool initialActivated;

    [Foldout("Activate")]
    public bool colored;
    public bool transformed;

    public bool hasFakeActivate;
    public bool fakeActivated;
    public bool overrideNoControl;

    [Foldout("Tattoo")]
    public CharacterTattooMenu myTatMenu;
    public bool menuFirstTriggered;
    public bool stageColoring;


    [Foldout("CharRefs")]
    public Transform npcMesh;
    public RiggedVisibleDetector visibleDetector;
    public GameObject iconPos;


    [Foldout("Quests")]
    public bool questTriggered;
    public bool questAccepted;

    [Foldout("Destinations")]
    public int _counter;
    public Transform destinationRef;
    public Transform currentDestination;
    protected List<Transform> destinations = new List<Transform>();

    [Foldout("Dialogue")]
    public Transform currentDialogue;
    public bool talkable;
    public bool inConversation;
    protected Transform dialogueHolder;
    private bool inCD;
    private float talkCD = 1.5f;

    [Foldout("Idle")]
    public bool idling;
    public string idleAction;


    bool lookCoroutineRuning;
    protected Coroutine lookCoroutine;



    [Foldout("OnActivate")]
    public UnityEvent OnActivateEvent;


    [Foldout("Door Checking")]
    public bool inDoorRange;
    public float atDoorTime;
    public bool waitForDoor;
    public Door currentDoor;
    bool doorAnimSet;

    protected virtual void Awake()
    {
        machine = GetComponent<BaseStateMachine>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        for (int i = 0; i < destinationRef.childCount; i++)
        {
            destinations.Add(destinationRef.GetChild(i));
        }
        currentDestination = destinations[_counter];
        transform.position = currentDestination.position;

    }

    protected virtual void Start()
    {
        //Setting up basic components

        player = ReferenceTool.player;
        playerHolding = ReferenceTool.playerHolding;

        anim.SetTrigger(machine.destinationData[_counter].idleTrigger);
        dialogueHolder = transform.GetChild(2);
        currentDialogue = dialogueHolder.GetChild(0);
    
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.name.Contains("Head") && !child.name.Contains("HeadTop"))
                head = child;
        }
        fadeInterval = 5;
        //agent.enabled = true;
    }

    public void OnEnable()
    {
        NPCSaveControl.npcActiveDict[this].activeNPC = true;
    }

    public void OnDisable()
    {
        NPCSaveControl.npcActiveDict[this].activeNPC = false;
    }


    protected virtual void Update()
    {
        currentDestination = destinations[_counter];
        NPCSaveControl.npcActiveDict[this].stage = _counter;

        if (_counter == destinations.Count)
        {
            finalStop = true;
        }

        isVisible = visibleDetector.isVisible;
        interactable = CheckInteractable();
        if (allowLookPlayer)
        {
            LookAtPlayer();
        }

        if (inLookCooldown)
        {
            if (lookCooldown > 0)
            {
                lookCooldown -= Time.deltaTime;
            }
            else
            {
                inLookCooldown = false;
                allowLookPlayer = false;
                lookCooldown = 10f;
            }
        }

        CheckTalkCD();



        #region Door Region

        if (inDoorRange && !waitForDoor && !currentDoor.doorOpen)
        {
            atDoorTime += Time.deltaTime;
            if (atDoorTime > 0.5f)
            {
                waitForDoor = true;
                atDoorTime = 0;
            }
        }

        if (waitForDoor)
        {
            NPCOpenDoor(currentDoor);
        }

        #endregion

        if (stageColoring)
        {
            ActivateAll(npcMesh);
        }

        if (interactable)
        {
            if ((!StartSequence.noControl || overrideNoControl) && !playerHolding.inDialogue && !MainTattooMenu.tatMenuOn)
            {
                TriggerConversation();
            }
        }
        else if (!inConversation)
        {
            if (!colored)
                ChangeLayer(0);
            else
                ChangeLayer(17);

        }

    }


    public void TransformNPC()
    {
        transformed = true;
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

    public void ColorActualNPC()
    {
        stageColoring = true;
    }


    public void ActivateAll(Transform obj)
    {
        if(matColorVal > 0f)
        {
            matColorVal -= 0.5f * Time.deltaTime;
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
        else
        {
            matColorVal = 0;
            stageColoring = false;
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
        material.SetFloat("_WhiteDegree", matColorVal);
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




    #region Idle Region

    public void StopIdle()
    {
        idling = false;
    }


    public void InvokeIdleFunction()
    {
        if(_counter > 0)
        {
            Invoke(idleAction, 0);
        }

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
        if (layerNumber == 9 || layerNumber == 21)
        {
            playerHolding.talkingTo = iconPos;
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
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        float time = 0;
        while (time < 2)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * 0.1f;
            yield return null;
        }
    }
    #endregion


    #region Conversation Control

    void TriggerConversation()
    {
        string reTriggerName = "NPC_" + gameObject.name + "_Other_Interacted";

        //check npc conditions to trigger conversation
        if (talkable && !inCD && !inConversation)
        {
            //if npc in idle states are already talked once 
            if (!DialogueLua.GetVariable(reTriggerName).asBool)
            {
                //move player to interactable layer
                if (colored)
                    ChangeLayer(9);
                else
                    ChangeLayer(21);
                //check player interaction command
                if (Input.GetMouseButtonDown(0))
                {
                    StartTalking();
                }
            }
        }
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

    public void SetTalkable()
    {
        talkable = true;
    }


    public void QuestAccept()
    {
        questAccepted = true;
    }



    protected virtual void QuestAcceptChange()
    {

    }


    protected virtual void OnConversationStart(Transform other)
    {
        inConversation = true;
        allowLookPlayer = true;
        inLookCooldown = false;
        if (colored)
        {
            ChangeLayer(17);
        }
        else
        {
            ChangeLayer(0);
        }

    }



    protected virtual void OnConversationEnd(Transform other)
    {
        inLookCooldown = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        inConversation = false;
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        inCD = true;
        currentDialogue.gameObject.SetActive(false);

        if (transformed)
        {
            if(!myTatMenu.finished)
                myTatMenu.StartFinalTattoo();

        }
    }

    #endregion

    #region Tattoo Region
    protected virtual void ActivateTattooMenu()
    {
        Invoke(nameof(DelayTurnOnMenu), 1f);
    }
    protected virtual void ActivateFinalTattoo()
    {

    }

    void DelayTurnOnMenu()
    {
        myTatMenu.BlinkMenuOn();
    }
    public void ChangeCharMeshMat(Material mat)
    {
        if (npcMesh.GetComponent<Renderer>())
        {
            npcMesh.GetComponent<Renderer>().material = mat;
        }
        foreach (Transform child in npcMesh)
        {
            if(child.GetComponent<Renderer>())
                child.GetComponent<Renderer>().material = mat;
        }
    }

    public void LoadCharMat(Material mat, bool colored)
    {
        if (npcMesh.GetComponent<Renderer>())
        {
            Renderer rend = npcMesh.GetComponent<Renderer>();
            rend.material = mat;
            if(colored)
                rend.material.SetFloat("_WhiteDegree", 0);
        }
        foreach (Transform child in npcMesh)
        {
            Renderer rend = child.GetComponent<Renderer>();
            if (rend)
            {
                rend.material = mat;
                if(colored)
                    rend.material.SetFloat("_WhiteDegree", 0);
            }
                
        }
    }


    #endregion

    #region Look At Player
    bool inDistance, inAngle;
    float lookWeight;
    Transform head;

    [Foldout("PlayerLook")]
    public bool allowLookPlayer;
    bool inLookCooldown;
    float lookCooldown = 10f;
    public float lookPlayerDist = 20;
    public float lookPlayerAngle = 45;
    public float lookPlayerSpeed = 2;

    void LookAtPlayer()
    {

        if ((Camera.main.transform.position - transform.position).sqrMagnitude < lookPlayerDist)
        {
            inDistance = true;
            if (Mathf.Abs(Vector3.SignedAngle(transform.forward, Camera.main.transform.position - head.position, Vector3.up)) < lookPlayerAngle)
                inAngle = true;
            else
                inAngle = false;
        }
        else
            inDistance = false;

        if (inDistance && inAngle)
            lookWeight = Mathf.Lerp(lookWeight, 1, lookPlayerSpeed * Time.deltaTime);
        else
            lookWeight = Mathf.Lerp(lookWeight, 0, lookPlayerSpeed * Time.deltaTime);


    }
    private void OnAnimatorIK()
    {
        if (allowLookPlayer)
        {
            anim.SetLookAtWeight(lookWeight);
            anim.SetLookAtPosition(Camera.main.transform.position);
        }
        else
        {
            if (lookWeight > 0)
                lookWeight -= .05f;
            else
                lookWeight = 0;
            anim.SetLookAtWeight(lookWeight);
        }
    }
    #endregion

    public void IdleRotate()
    {
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);

        if (destinations.Count > 0 && _counter > 0)
            lookCoroutine = StartCoroutine(RotateTowards(destinations[_counter].transform.GetChild(0).transform));
    }

    public void TalkRotate()
    {

        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        lookCoroutine = StartCoroutine(RotateTowards(player));
    }

    public void StopLookRotation()
    {
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
    }

    #region AI Region

    public bool FinalStop()
    {
        if (_counter == destinations.Count)
            return true;
        return false;
    }
    public Transform GetNext()
    {

        var point = destinations[_counter];

        return point;
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



    public void SetWaitAction()
    {
        idleAction = gameObject.name + "Action" + _counter;
    }

    public void SetNPCPosition()
    {
        transform.position = destinations[_counter].position;
    }

    public bool AtLastStop()
    {
        return _counter == destinations.Count;
    }

    public void SetDialogue()
    {

        currentDialogue = dialogueHolder.GetChild(_counter);
        SetMainTalkFalse();

    }

    public void SetMainTalkeTrue()
    {
        string seTalkName = "NPC/" + gameObject.name + "/Main_Talked";
        DialogueLua.SetVariable(seTalkName, true);
    }

    public void SetMainTalkFalse()
    {
        string seTalkName = "NPC/" + gameObject.name + "/Main_Talked";
        DialogueLua.SetVariable(seTalkName, false);
        string reTriggerName = "NPC/" + gameObject.name + "/Other_Interacted";
        DialogueLua.SetVariable(reTriggerName, false);
    }

    public bool GetMainTalked()
    {
        string seTalkName = "NPC/" + gameObject.name + "/Main_Talked";
        return DialogueLua.GetVariable(seTalkName).asBool;
    }


    public void NPCOpenDoor(Door door)
    {
        if (!HasReached(agent))
        {
            agent.isStopped = true;
            if (!door.doorOpen)
            {
                if (!doorAnimSet)
                {
                    Invoke(nameof(SetDoorOpen), 1f);
                    anim.SetTrigger("OpenDoor");
                    doorAnimSet = true;
                }


            }

            if(doorAnimSet && door.doorOpen)
            {
                doorAnimSet = false;
                DoorOpenMove();
            }

        }
    }

    void SetDoorOpen()
    {
        currentDoor.NPCOpenDoor();
    }

    void DoorOpenMove()
    {

        inDoorRange = false;
        currentDoor = null;
        waitForDoor = false;
        anim.SetTrigger("Move");
        agent.isStopped = false;
    }


    #endregion


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(("DoorDetector")))
        {
            Door newDoor = other.transform.parent.GetComponent<Door>();
            if (newDoor.enabled)
            {
                if (!newDoor.doorOpen)
                {
                    inDoorRange = true;
                    currentDoor = other.transform.parent.GetComponent<Door>();
                }
            }

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DoorDetector"))
        {
            inDoorRange = false;
            if (!waitForDoor)
                currentDoor = null;
            atDoorTime = 0;
        }
    }




}
