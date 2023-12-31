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
    public CinemachineVirtualCamera loyiCam;

    public Kelvin kelvin;
    public Felix felix;

    public StudioEventEmitter izaInEmitter;
    bool fadingIzaAmbience;
    float izaInFadeVal;

    LivableObject[] livObjects;
    bool tabHintOn;

    CinemachinePOV loyiCamPOV;
    CinemachinePOV akiCamPOV;
    

    // Start is called before the first frame update
    void Start()
    {
        groupColorVal = 0;
        livObjects = FindObjectsOfType<LivableObject>();
        noControl = false;

        loyiCamPOV = loyiCam.GetCinemachineComponent<CinemachinePOV>();
        akiCamPOV = tableCam.GetCinemachineComponent<CinemachinePOV>();
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
        loyiCam.m_Priority = 9;
    }

    public void ResetShockCamera()
    {

        tableCam.m_Priority = 10;
        loyiCam.m_Priority = 11;
        shockCam.m_Priority = 0;
    }

    public void LoyiCamOn()
    {
        loyiCam.m_Priority = 10;
        tableCam.m_Priority = 9;
    }

    public void ResetLoyiCam()
    {
        loyiCamPOV.m_HorizontalAxis.Value = 180;
        loyiCamPOV.m_VerticalAxis.Value = 0;
    }

    public void ResetAkiCam()
    {
        akiCamPOV.m_VerticalAxis.Value = 0;
        akiCamPOV.m_HorizontalAxis.Value = 270;
    }

    public void AkiCam()
    {
        tableCam.m_Priority = 10;
        loyiCam.m_Priority = 9;
    }

    public void IzaPrologueFinalCam()
    {
        loyiCam.m_Priority = 8;
    }
}
