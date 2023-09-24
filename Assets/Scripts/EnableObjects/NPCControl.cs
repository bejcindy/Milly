using NPCFSM;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class NPCControl : MonoBehaviour
{

    private float matColorVal;
    [Header("[Activate Check]")]
    public bool initialActivated;
    public bool npcActivated;
    [SerializeField] protected float fadeInterval;
    [SerializeField] protected float minDist;
    [SerializeField] protected bool isVisible;
    [SerializeField] protected bool interactable;
    [SerializeField] public bool followingPlayer;
    [SerializeField] protected float npcVincinity;

    public Transform npcMesh;
    public RiggedVisibleDetector visibleDetector;

    public LivableObject triggerObject;


    protected Transform player;
    protected DialogueSystemTrigger dialogue;
    protected Animator anim;
    protected NavMeshAgent agent;
    protected BaseStateMachine machine;




    protected bool firstTalk;

    [Header("[Route Control]")]
    public Transform[] destinations;
    public Component[] destObjects;
    public float[] waitTimes;

    public int _counter = 0;
    public bool finalStop;
    Transform currentStop;

    [Header("[Conversation]")]
    public bool inConversation;
    public bool inCD;
    public bool reTriggerConversation;
    private float talkCD = 2f;



    [Header("[Idle State]")]
    public float idleTime;
    protected string idleAction;
    public bool idling;
    public bool idlePaused;
    public bool idleComplete;
    
    
    protected Coroutine lookCoroutine;
    bool lookCoroutineRuning;
    
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Setting up basic components
        player = GameObject.Find("Player").transform;
        matColorVal = 1f;
        if (initialActivated)
            npcActivated = true;

        machine = GetComponent<BaseStateMachine>();
        dialogue = GetComponent<DialogueSystemTrigger>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        //Basic visible and interactable detection
        isVisible = visibleDetector.isVisible;
        interactable = CheckInteractable();

        CheckNPCActivation();
        CheckTalkCD();
        CheckIdle();

        Debug.Log(gameObject.name + " " + lookCoroutineRuning);


        if (npcActivated)
        {
            if(matColorVal > 0f)
                ActivateAll(npcMesh);
        }
            


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



    #region ActivateFunctionality
    void CheckNPCActivation()
    {
        if (triggerObject != null && triggerObject.activated)
            npcActivated = true;
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

    public void ActivateDesignatedObject()
    {
        if (Vector3.Distance(transform.position, player.position) < npcVincinity)
        {
            if (destObjects[_counter - 1].transform.GetComponent<LivableObject>())
                destObjects[_counter - 1].transform.GetComponent<LivableObject>().activated = true;
            if (destObjects[_counter - 1].transform.GetComponent<BuildingGroupController>())
                destObjects[_counter - 1].transform.GetComponent<BuildingGroupController>().activateAll = true;
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

    void ChangeLayer(int layerNumber)
    {
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
    void OnConversationStart(Transform other)
    {
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        lookCoroutine = StartCoroutine(RotateTowards(player));
        if (!firstTalk)
        {
            firstTalk = true;
        }

        inConversation = true;
    }

    void OnConversationEnd(Transform other)
    {
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        inConversation = false;
        inCD = true;
        reTriggerConversation = false;
        dialogue.enabled = false;

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
    }

    public bool HasReached(NavMeshAgent agent)
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.isStopped = true;
                    return true;
                }
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
    #endregion

}
