using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using static Cinemachine.CinemachineOrbitalTransposer;
using VInspector;

public class LivableObject : MonoBehaviour
{

    protected Transform player;
    protected PlayerHolding playerHolding;
    protected PlayerLeftHand playerLeftHand;
    protected Material mat;
    protected CinemachineVirtualCamera playerCam;


    protected bool checkVisible;
    protected bool lookingType;
    protected bool doorType;
    protected bool pickType;
    protected bool hasGroupControl;
    protected GroupMaster groupControl;
    protected Vector3 pointOnScreen;
    protected bool tableObj;

    [Foldout("State")]

    public bool activated;
    public bool transformed;
    private bool firstActivated;

    [SerializeField] public bool interactable;
    [SerializeField] protected bool isVisible;


    [Foldout("Basic")]
    [SerializeField] protected float minDist;
    [SerializeField] protected Renderer rend;

    [SerializeField] protected float matColorVal;
    [SerializeField] protected float fadeInterval;

    [Foldout("Tattoo")]
    [SerializeField] protected Tattoo myTat;
    [SerializeField] protected bool delayTat;

    [Foldout("Effects")]
    [SerializeField] protected GameObject specialEffect;
    [SerializeField] protected GameObject postProcessingVolume;


    [Foldout("Rigged")]
    [SerializeField] protected bool rigged;
    [SerializeField] protected RiggedVisibleDetector visibleDetector;


    [Foldout("View Detection")]
    public bool centerFocused;
    public bool noRend;
    public bool onlyFront = true;
    static float allowedAngle = .2f;

    [Foldout("Special")]
    public bool overrideStartSequence;
    [SerializeField] private bool izaProp;




    [Foldout("Activate Event")]
    public UnityEvent OnActivateEvent;

    bool[] checkBoundVisible;



    [SerializeField] private ScriptController myZone;
    public bool scriptOff;

    protected virtual void Start()
    {
        player = ReferenceTool.player;
        playerHolding = ReferenceTool.playerHolding;
        playerLeftHand = ReferenceTool.playerLeftHand;
        postProcessingVolume = GameObject.Find("GlowVolume");
        if (GetComponent<Renderer>() != null)
        {
            rend = GetComponent<Renderer>();
        }
        if (!noRend)
        {
            mat = rend.material;
            mat.EnableKeyword("_WhiteDegree");
        }

        if(GetComponent<GroupMaster>() != null)
        {
            hasGroupControl = true;
            groupControl = GetComponent<GroupMaster>();
        }
        checkBoundVisible = new bool[8];
        playerCam = ReferenceTool.playerCinemachine;
    }

    protected virtual void Update()
    {
        if (transformed)
        {
            if (gameObject.layer == 6 || gameObject.layer == 18)
                gameObject.layer = 18;
            else
                gameObject.layer = 17;
        }

        if (myZone != null)
        {
            if (myZone.inZone)
                scriptOff = false;
            else
                scriptOff = true;
        }

        if ((!StartSequence.noControl || overrideStartSequence) && !scriptOff && !MainTattooMenu.tatMenuOn)
        {
            if (!doorType && rend)
            {
                checkVisible = IsObjectVisible(rend);
                if (!rigged)
                {
                    if (checkVisible)
                    {
                        isVisible = IsInView();
                    }
                    else
                        isVisible = false;
                }
                else
                {
                    isVisible = visibleDetector.isVisible;
                }
            }
            DetectInteractable();


            if (activated)
            {
                if (!firstActivated)
                {
                    if (lookingType)
                    {
                        if (DataHolder.camBlended && DataHolder.camBlendDone)
                            TurnOnColor(mat);
                    }
                    else
                    {
                        TurnOnColor(mat);
                    }

                    if (OnActivateEvent != null)
                    {
                        OnActivateEvent.Invoke();
                    }

                }

                if (hasGroupControl && matColorVal <= 0)
                {
                    groupControl.activateAll = true;
                }

                //change layer related code
                if ((!StartSequence.noControl || overrideStartSequence) && !izaProp)
                {
                    if (gameObject.layer != 17 && gameObject.layer != 18)
                    {
                        if (gameObject.layer == 6 || gameObject.layer == 18)
                        {
                            gameObject.layer = 18;
                        }
                        else
                        {
                            gameObject.layer = 17;
                        }

                    }
                }



            }

        }



    }

    protected virtual void DetectInteractable()
    {
        if (Vector3.Distance(transform.position, player.position) <= minDist || Vector3.Distance(transform.position, Camera.main.transform.position) <= minDist)
        {
            if (isVisible)
                interactable = true;
            else
                interactable = false;
        }
        else
        {
            interactable = false;
        }
    }

    FMOD.Studio.EventInstance snapshot;
    float I;
    bool played;
    bool fadeOut;
    protected virtual void TurnOnColor(Material material)
    {
        if (matColorVal > 0)
        {
            matColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            if (material.HasFloat("_WhiteDegree"))
                material.SetFloat("_WhiteDegree", matColorVal);
            //Audio Related Stuff
            if (!played)
            {
                if (pickType)
                {
                    snapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/EnableObject");
                    snapshot.start();
                }
                played = true;
            }
        }
        else
        {
            //fadeOut = true;
            matColorVal = 0;
            firstActivated = true;
            if (myTat && !delayTat)
                Invoke(nameof(TurnOnTat), 1f);
            if (specialEffect != null)
                specialEffect.SetActive(true);
        }
    }

    void TurnOnTat()
    {
        myTat.myPanel.mainTattooMenu.activePanel = myTat.myPanel;
        myTat.myPanel.mainTattooMenu.showPanel = true;
        myTat.activated = true;
    }

    public void Activate()
    {
        activated = true;
    }

    public void TransformColor()
    {
        transformed = true;
    }

    public void EnableInteract()
    {
        overrideStartSequence = true;
    }


    void FadeOutFilter()
    {
        if (I > 0)
        {
            I -= 0.1f * fadeInterval * Time.deltaTime;
            snapshot.setParameterByName("EnableFilterIntensity", I);
        }
        else
        {
            I = 0;
            snapshot.setParameterByName("EnableFilterIntensity", I);
            snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            snapshot.release();
            fadeOut = false;
        }
    }

    protected virtual bool IsInView()
    {
        pointOnScreen = Camera.main.WorldToScreenPoint(rend.bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {
            return false;
        }

        //Is in FOV
        if (centerFocused)
        {
            int pointsInScreen = 0;
            Vector3 pointA = rend.bounds.min;
            Vector3 pointB = rend.bounds.min + new Vector3(rend.bounds.size.x, 0, 0);
            Vector3 pointC = rend.bounds.min + new Vector3(0, rend.bounds.size.y, 0);
            Vector3 pointD = rend.bounds.min + new Vector3(0, 0, rend.bounds.size.z);
            Vector3 pointE = rend.bounds.max - new Vector3(rend.bounds.size.x, 0, 0);
            Vector3 pointF = rend.bounds.max - new Vector3(0, rend.bounds.size.y, 0);
            Vector3 pointG = rend.bounds.max - new Vector3(0, 0, rend.bounds.size.z);
            Vector3 pointH = rend.bounds.max;


            checkBoundVisible[0] = CheckPointInView(pointA);
            checkBoundVisible[1] = CheckPointInView(pointB);
            checkBoundVisible[2] = CheckPointInView(pointC);
            checkBoundVisible[3] = CheckPointInView(pointD);
            checkBoundVisible[4] = CheckPointInView(pointE);
            checkBoundVisible[5] = CheckPointInView(pointF);
            checkBoundVisible[6] = CheckPointInView(pointG);
            checkBoundVisible[7] = CheckPointInView(pointH);

            for (int i = 0; i < checkBoundVisible.Length; i++)
            {
                if (checkBoundVisible[i])
                    pointsInScreen++;
            }

            if (pointsInScreen < 3)
                return false;

        }
        else if (tableObj)
        {
            if ((pointOnScreen.x < Screen.width * 0.4f) || (pointOnScreen.x > Screen.width * 0.6f) ||
                (pointOnScreen.y < Screen.height * 0.4f) || (pointOnScreen.y > Screen.height * 0.6f))
            {
                return false;
            }
        }
        else
        {
            if ((pointOnScreen.x < Screen.width * 0.2f) || (pointOnScreen.x > Screen.width * 0.8f) ||
               (pointOnScreen.y < Screen.height * 0.2f) || (pointOnScreen.y > Screen.height * 0.8f))
            {
                return false;
            }

        }

        if (!centerFocused)
        {
            if (rend != null)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position - new Vector3(0, 0.1f, 0), rend.bounds.center - (Camera.main.transform.position - new Vector3(0, 0.1f, 0)), out hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.name != gameObject.name && !hit.collider.CompareTag("Player"))
                    {
                        return false;
                    }

                }
            }
        }

        return true;
    }

    bool CheckPointInView(Vector3 pointPos)
    {
        Vector3 pointOnScreen = Camera.main.WorldToScreenPoint(pointPos);
        if ((pointOnScreen.x < Screen.width * 0.05f) || (pointOnScreen.x > Screen.width * 0.95f) ||
           (pointOnScreen.y < Screen.height * 0.05f) || (pointOnScreen.y > Screen.height * 0.95f))
        {
            return false;
        }
        return true;
    }
    public static bool IsObjectVisible(Renderer renderer)
    {
        Transform go = renderer.transform;
        if (go.GetComponent<LookingObject>() && go.GetComponent<LookingObject>().onlyFront)
        {
            if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), renderer.bounds))
            {
                Transform playerT = GameObject.Find("Player").transform;
                LookingObject lo;
                lo = go.GetComponent<LookingObject>();
                if ( Vector3.Distance(go.position, playerT.position) <= lo.minDist)
                {
                    //get direction
                    if (Vector3.Dot(playerT.position - go.position, go.forward) > allowedAngle)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }
        else
        {
            return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), renderer.bounds);
        }
    }
    protected virtual bool isDoorHandleVisible(Collider col)
    {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), col.bounds);
    }



}
