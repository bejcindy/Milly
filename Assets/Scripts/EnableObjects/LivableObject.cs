using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class LivableObject : MonoBehaviour
{
    protected Transform player;
    [SerializeField] public bool interactable;
    [SerializeField] protected bool isVisible;
    [SerializeField] protected bool checkVisible;
    [SerializeField] protected bool chainControl;
    [SerializeField] protected float minDist;


    protected Material mat;
    [SerializeField] protected Renderer rend;
    [SerializeField] protected Collider coll;
    [SerializeField] public bool activated;
    [SerializeField] public bool firstActivated; 
    [SerializeField] protected float matColorVal;
    [SerializeField] protected float fadeInterval;

    [Header("Effects")]
    [SerializeField] protected GameObject specialEffect;
    [SerializeField] protected GameObject postProcessingVolume;
    [SerializeField] protected CinemachineVirtualCamera playerCam;

    [Header("Rigged")]
    [SerializeField] protected bool rigged;
    [SerializeField] protected RiggedVisibleDetector visibleDetector;

    public bool centerFocused;

    public bool npc;

    bool[] checkBoundVisible;


    protected virtual void Start()
    {
        player = GameObject.Find("Player").transform;
        postProcessingVolume = GameObject.Find("GlowVolume");
        if (GetComponent<Renderer>() != null)
        {
            rend = GetComponent<Renderer>();
        }
        if (!npc)
        {
            mat = rend.material;
            mat.EnableKeyword("_WhiteDegree");
            mat.SetFloat("_WhiteDegree", 1);
        }
        checkBoundVisible = new bool[8];
        matColorVal = 1;
        playerCam = GameObject.Find("PlayerCinemachine").GetComponent<CinemachineVirtualCamera>();
    }

    protected virtual void Update()
    {
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

        DetectInteractable();
        if(!npc)
            matColorVal = mat.GetFloat("_WhiteDegree");




        //isVisible = IsInView();
        if (activated)
        {
            if (!firstActivated)
            {
                TurnOnColor(mat);
            }
                
            if (chainControl)
            {
                GetComponent<GroupMaster>().activateAll = true;
            }
                
        }
    }

    protected virtual void DetectInteractable()
    {
        if (Vector3.Distance(transform.position, player.position) <= minDist)
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
            firstActivated = true;
            if (specialEffect != null)
                specialEffect.SetActive(true);
        }
    }

    protected virtual bool IsInView()
    {
        Vector3 pointOnScreen = Camera.main.WorldToScreenPoint(rend.bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {
            if(gameObject.name.Contains("pizza"))
                Debug.Log("Behind: " + gameObject.name);
            return false;
        }

        //Is in FOV
        if (centerFocused)
        {
            //if ((pointOnScreen.x < Screen.width * 0.4f) || (pointOnScreen.x > Screen.width * 0.6f) ||
            //    (pointOnScreen.y < Screen.height * 0.4f) || (pointOnScreen.y > Screen.height * 0.6f))
            //{
            //    return false;
            //}
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
                else
                    pointsInScreen--;
            }

            if (pointsInScreen < 2)
                return false;
        }
        else
        {
            if ((pointOnScreen.x < Screen.width * 0.2f) || (pointOnScreen.x > Screen.width * 0.8f) ||
               (pointOnScreen.y < Screen.height * 0.2f) || (pointOnScreen.y > Screen.height * 0.8f))
            {
                return false;
            }
            //int pointsInScreen = 0;
            //Vector3 pointA = rend.bounds.min;
            //Vector3 pointB = rend.bounds.min + new Vector3(rend.bounds.size.x, 0, 0);
            //Vector3 pointC = rend.bounds.min + new Vector3(0, rend.bounds.size.y, 0);
            //Vector3 pointD = rend.bounds.min + new Vector3(0, 0, rend.bounds.size.z);
            //Vector3 pointE = rend.bounds.max - new Vector3(rend.bounds.size.x, 0, 0);
            //Vector3 pointF = rend.bounds.max - new Vector3(0, rend.bounds.size.y, 0);
            //Vector3 pointG = rend.bounds.max - new Vector3(0, 0, rend.bounds.size.z);
            //Vector3 pointH = rend.bounds.max;


            //checkBoundVisible[0] = CheckPointInView(pointA);
            //checkBoundVisible[1] = CheckPointInView(pointB);
            //checkBoundVisible[2] = CheckPointInView(pointC);
            //checkBoundVisible[3] = CheckPointInView(pointD);
            //checkBoundVisible[4] = CheckPointInView(pointE);
            //checkBoundVisible[5] = CheckPointInView(pointF);
            //checkBoundVisible[6] = CheckPointInView(pointG);
            //checkBoundVisible[7] = CheckPointInView(pointH);

            //for (int i = 0; i < checkBoundVisible.Length; i++)
            //{
            //    if (checkBoundVisible[i])
            //        pointsInScreen++;
            //    else
            //        pointsInScreen--;
            //}

            //if (pointsInScreen < 3)
            //    return false;
        }


        //RaycastHit hit;
        //if (Physics.Raycast(Camera.main.transform.position, GetComponent<Renderer>().bounds.center, out hit))
        //{
        //    if (hit.collider.name != gameObject.name && !hit.collider.CompareTag("Player"))
        //    {
        //        //Debug.Log(gameObject.name + "blocked by " + hit.collider.name);
        //        return false;
        //    }

        //}
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

    protected virtual void OnBecameVisible()
    {
        checkVisible = true;
    }

    protected virtual void OnBecameInvisible()
    {
        checkVisible = false;
    }
}
