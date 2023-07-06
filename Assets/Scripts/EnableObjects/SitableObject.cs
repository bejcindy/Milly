using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static UnityEngine.GraphicsBuffer;

public class SitableObject : FixedCameraObject
{
    public PlayerCam camController;
    public PlayerMovement playerMovement;
    Renderer playerBody;


    protected override void Start()
    {
        base.Start();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerBody = player.GetChild(0).GetComponent<Renderer>();
    }


    public void PositionPlayer()
    {
        playerMovement.enabled = false;
        playerBody.enabled = false;
        fixedCamera.m_Priority = 10;
        playerCamera.m_Priority = 9;
    }



}
