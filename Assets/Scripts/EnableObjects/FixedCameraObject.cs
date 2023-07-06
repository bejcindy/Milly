using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FixedCameraObject : LivableObject
{
    [SerializeField] protected KeyCode interactKey;
    [SerializeField] protected KeyCode quitKey;
    [SerializeField] protected bool isInteracting;
    [SerializeField] protected bool positionFixed;

    [SerializeField] protected CinemachineVirtualCamera fixedCamera;
    [SerializeField] protected CinemachineVirtualCamera playerCamera;

    PlayerCam camController;
    PlayerMovement playerMovement;
    Renderer playerBody;

    protected override void Start()
    {
        base.Start();
        playerMovement = player.GetComponent<PlayerMovement>();
        camController = player.GetComponent<PlayerCam>();
        playerBody = player.GetChild(0).GetComponent<Renderer>();
    }



    protected override void Update()
    {
        base.Update();
        if (isVisible)
        {
            if (!isInteracting)
            {
                TriggerInteraction();
            }
        }

        if (positionFixed)
        {
            if (Input.GetKeyDown(quitKey))
            {
                StartCoroutine(UnfixPlayer());
            }
        }

    }


    protected void TriggerInteraction()
    {
        if (Input.GetKeyDown(interactKey))
        {
            isInteracting = true;
            activated = true;
            positionFixed = true;
            PositionPlayer();
        }
    }

    protected void PositionPlayer()
    {
        playerMovement.enabled = false;
        playerBody.enabled = false;
        fixedCamera.m_Priority = 10;
        playerCamera.m_Priority = 9;
    }


    IEnumerator UnfixPlayer()
    {
        playerCamera.m_Priority = 10;
        fixedCamera.m_Priority = 9;
        yield return new WaitForSeconds(2f);
        playerBody.enabled = true;
        camController.enabled = true;
        playerMovement.enabled = true;
        positionFixed = false;
        isInteracting = false;
    }
}
