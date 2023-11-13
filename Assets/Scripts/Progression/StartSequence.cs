using Cinemachine;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSequence : MonoBehaviour
{

    public static bool noControl;

    public bool activateAll;
    public float targetVal;
    public float groupColorVal;

    public CinemachineVirtualCamera tableCam;
    public CinemachineVirtualCamera shockCam;

    public StudioEventEmitter izaInEmitter;
    bool fadingIzaAmbience;
    float izaInFadeVal;

    LivableObject[] livObjects;
    bool tabHintOn;
    // Start is called before the first frame update
    void Start()
    {
        groupColorVal = 0;
        livObjects = FindObjectsOfType<LivableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        IzaInteriorFade();
        if (Input.GetKeyDown(KeyCode.Tab) && tabHintOn)
        {
            DataHolder.HideHint(DataHolder.hints.tabHint);
            tabHintOn = false;
        }
    }

    public void IzaInteriorFade()
    {
        if (activateAll)
        {
            if (groupColorVal < targetVal)
                groupColorVal += Time.deltaTime;

            ActivateAll(this.transform);

        }

        if (fadingIzaAmbience)
            FadeIzaInAmbience();
    }

    void ActivateAll(Transform obj)
    {
        if (obj.GetComponent<Renderer>() != null)
        {
            Material mat = obj.GetComponent<Renderer>().material;
            if (mat.HasProperty("_WhiteDegree"))
            {
                mat.EnableKeyword("_WhiteDegree");
                if (mat.GetFloat("_WhiteDegree") <= targetVal)
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
                    if (childMat.GetFloat("_WhiteDegree") <= targetVal)
                        TurnOnColor(childMat);
                }

            }
            else
            {
                ActivateAll(child);
            }

        }

    }

    void TurnOnColor(Material material)
    {
        material.SetFloat("_WhiteDegree", groupColorVal);

    }
    public void ShowTabHint()
    {
        DataHolder.ShowHint(DataHolder.hints.tabHint);
        tabHintOn = true;
    }
    public void FadeIzaInAmbience()
    {
        fadingIzaAmbience = true;
        if(izaInFadeVal < 5)
        {
            izaInFadeVal += Time.deltaTime;
            izaInEmitter.SetParameter("IzaInFade", izaInFadeVal);
        }
        else
        {
            fadingIzaAmbience = false;
        }
    }

    public void SetTargetVal(float target)
    {
        targetVal = target;
    }

    public void SetStartFade()
    {
        activateAll = true;
    }

    public void StopStartFade()
    {
        activateAll = false;
    }

    public void DeActivateAll()
    {
        noControl = true;
    }

    public void ReactivateAll()
    {
        noControl = false;
    }

    public void SetShockCamera()
    {
        if (tabHintOn)
        {
            DataHolder.HideHint(DataHolder.hints.tabHint);
            tabHintOn = false;
        }
        shockCam.m_Priority = 10;
        tableCam.m_Priority = 9;
    }

    public void ResetShockCamera()
    {
        tableCam.m_Priority = 10;
        shockCam.m_Priority = 0;
    }
}
