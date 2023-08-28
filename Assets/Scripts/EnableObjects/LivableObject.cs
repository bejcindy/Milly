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


        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, GetComponent<Renderer>().bounds.center, out hit))
        {
            if (hit.collider.name != gameObject.name && !hit.collider.CompareTag("Player"))
            {
                Debug.Log(gameObject.name + "blocked by " + hit.collider.name);
                return false;
            }

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
