using NPCFSM;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCControl : LivableObject
{
    [Header("Checkers")]
    public float npcVincinity;

    public bool firstTalk;
    public bool npcActivated;
    public bool inConversation;
    public bool inCD;
    public bool reTriggerConversation;
    private float talkCD = 2f;



    [Header("Idle State")]
    public float idleTime;
    public string idleAction;
    public bool idling;
    public bool idlePaused;
    public bool idleComplete;


    
    public LivableObject triggerObject;
    public Transform npcMesh;
    NPCDestinations npcDestinations;

    DialogueSystemTrigger dialogue;
    Transform[] destinations;
    Animator anim;
    protected Coroutine lookCoroutine;
    bool lookCoroutineRuning;
    NavMeshAgent agent;
    public BaseStateMachine machine;

    protected Dictionary<Transform, string> destToAction = new Dictionary<Transform, string>();
    protected Dictionary<Transform, float> destToTime = new Dictionary<Transform, float>();
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        npcDestinations = GetComponent<NPCDestinations>();
        machine = GetComponent<BaseStateMachine>();
        dialogue = GetComponent<DialogueSystemTrigger>();
        destinations = npcDestinations.GetDestinations();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SetUpDictionary();
    }

    protected override void Update()
    {
        base.Update();
        CheckTalkCD();
        CheckIdle();

        Debug.Log(lookCoroutineRuning);
        
        if (triggerObject.activated)
            npcActivated = true;

        if (npcActivated)
            ActivateAll(npcMesh);


        if(!inConversation && interactable && !inCD)
        {
            ChangeLayer(9);
            if (Input.GetMouseButtonDown(0))
                reTriggerConversation = true;
        }
        else
        {
            ChangeLayer(0);
        }


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

    void CheckIdle()
    {
        if (idling && !idlePaused)
        {
            if (idleTime > 0)
                idleTime -= Time.deltaTime;
            else
            {
                idling = false;
                StopCoroutine(lookCoroutine);
            }
        }
    }

    public void InvokeIdleFunction()
    {
        Invoke(idleAction, 0);
    }

    public void NpcFinished()
    {
        gameObject.SetActive(false);
    }


    void SetUpDictionary()
    {
        for (int i = 0; i < destinations.Length; i++)
        {
            destToAction.Add(destinations[i], destinations[i].name + "Action");
        }
    }

    void ChangeLayer(int layerNumber)
    {
        foreach (Transform child in npcMesh)
        {
            child.gameObject.layer = layerNumber;
        }
    }



    //CONVERSATION CONTROL
    void OnConversationStart(Transform other)
    {
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        lookCoroutine = StartCoroutine(RotateTowards(player));
        if (!firstTalk)
        {
            firstTalk = true;
            SetAnimatorTrigger("Start");
        }

        inConversation = true;
    }

    void OnConversationEnd(Transform other)
    {
        StopCoroutine(lookCoroutine);
        inConversation = false;
        inCD = true;
        reTriggerConversation = false;
        dialogue.enabled = false;

        if (!idling)
            SetAnimatorTrigger("Move");
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


    //ANIMATOR CONTROL
    public void SetAnimatorTrigger(string trigger)
    {
        foreach(var param in anim.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger && param.name != trigger)
                anim.ResetTrigger(param.name);
        }
        anim.SetTrigger(trigger);
    }

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
}
