using Cinemachine;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSequence : MonoBehaviour
{

    public static bool noControl;


    public CinemachineVirtualCamera tableCam;
    public CinemachineVirtualCamera shockCam;
    public CinemachineVirtualCamera loyiCam;

    CinemachinePOV loyiCamPOV;
    CinemachinePOV akiCamPOV;
    


    void Start()
    {
        noControl = false;

        loyiCamPOV = loyiCam.GetCinemachineComponent<CinemachinePOV>();
        akiCamPOV = tableCam.GetCinemachineComponent<CinemachinePOV>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void DeActivateAll()
    {
        noControl = true;
    }

    public void ReactivateAll()
    {
        noControl = false;
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
