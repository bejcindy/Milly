using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static UnityEngine.GraphicsBuffer;

public class SitableObject : LivableObject
{
    [SerializeField] public KeyCode interactKey;
    public PlayerCam camController;
    public PlayerMovement playerMovement;
    Renderer playerBody;

    public CinemachineVirtualCamera fixedCamera;
    public CinemachineVirtualCamera playerCamera;

    public bool interacted;
    public bool positionFixed;
    Vector3 fixedPosition = new Vector3 (0f, 0f, 1f);
    Vector3 fixedRotation = Vector3.zero;


    protected override void Start()
    {
        base.Start();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerBody = player.GetChild(0).GetComponent<Renderer>();
    }
    protected override void Update()
    {
        base.Update();
        if (!interacted)
        {
            if (Input.GetKeyDown(interactKey))
            {
                interacted = true;
                positionFixed = true;
                activated = true;
                //camController.enabled = false;
                PositionPlayer();
            }
        }
        if (positionFixed)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(UnfixPlayer());
            }
        }
    }

    public void PositionPlayer()
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
        interacted = false;
    }
}
