using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using static Cinemachine.CinemachineOrbitalTransposer;

public class LivableObject : MonoBehaviour
{
    protected Transform player;
    [SerializeField] public bool interactable;
    [SerializeField] protected bool isVisible;
    [SerializeField] protected bool checkVisible;
    [SerializeField] protected bool tableObj;
    [SerializeField] protected float minDist;


    protected Material mat;
    [SerializeField] protected Renderer rend;
    [SerializeField] public bool activated;
    [SerializeField] public bool firstActivated;
    [SerializeField] protected float matColorVal;
    [SerializeField] protected float fadeInterval;

    [Header("Effects")]
    [SerializeField] protected GameObject specialEffect;
    [SerializeField] protected GameObject postProcessingVolume;
    protected CinemachineVirtualCamera playerCam;

    [Header("Rigged")]
    [SerializeField] protected bool rigged;
    [SerializeField] protected RiggedVisibleDetector visibleDetector;

    public bool centerFocused;
    public bool noRend;
    public bool overrideStartSequence;

    [Header("Only Used For LookingObject")]
    public bool onlyFront = true;
    static float allowedAngle = .2f;

    [Header("On Activate")]
    public UnityEvent OnActivateEvent;
    bool[] checkBoundVisible;

    protected virtual void Start()
    {
        player = GameObject.Find("Player").transform;
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
        checkBoundVisible = new bool[8];
        matColorVal = 1;
        playerCam = GameObject.Find("PlayerCinemachine").GetComponent<CinemachineVirtualCamera>();
    }

    protected virtual void Update()
    {
        if (!StartSequence.noControl || overrideStartSequence)
        {
            if (!GetComponent<Door>() && rend)
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

            if (fadeOut)
            {
                if (GetComponent<PickUpObject>())
                    FadeOutFilter();
            }

            if (activated)
            {
                if (!firstActivated)
                {
                    if (GetComponent<LookingObject>())
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

                if (GetComponent<GroupMaster>() && matColorVal <= 0)
                {
                    GetComponent<GroupMaster>().activateAll = true;
                }

                //change layer related code
                //if(!StartSequence.noControl || overrideStartSequence)
                //{
                //    if (gameObject.layer != 17 && gameObject.layer != 18)
                //    {
                //        if (gameObject.layer == 6 || gameObject.layer == 18)
                //        {
                //            gameObject.layer = 18;
                //        }
                //        else
                //            gameObject.layer = 17;
                //    }
                //}
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
                if (GetComponent<PickUpObject>())
                {
                    snapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/EnableObject");
                    snapshot.start();
                }
                played = true;
            }
            I = 1 - matColorVal;
            if (GetComponent<PickUpObject>())
            {
                snapshot.setParameterByName("EnableFilterIntensity", I);
            }
        }
        else
        {
            fadeOut = true;
            matColorVal = 0;
            firstActivated = true;
            if (specialEffect != null)
                specialEffect.SetActive(true);
        }
    }

    public void Activate()
    {
        activated = true;
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
        Vector3 pointOnScreen = Camera.main.WorldToScreenPoint(rend.bounds.center);

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
            if (GetComponent<Renderer>())
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position - new Vector3(0, 0.1f, 0), GetComponent<Renderer>().bounds.center - (Camera.main.transform.position - new Vector3(0, 0.1f, 0)), out hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore))
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
                if (Vector3.Distance(go.position, playerT.position) <= lo.minDist)
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
