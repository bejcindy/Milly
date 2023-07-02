using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SitableObject : LivableObject
{
    [SerializeField] public KeyCode interactKey;
    public PlayerCam camController;
    public PlayerMovement playerMovement;

    Renderer playerBody;
    public GameObject fixedCinemachine;
    public bool interacted;
    Vector3 fixedPosition = new Vector3 (0f, 0f, 1f);
    Vector3 fixedRotation = Vector3.zero;


    protected override void Start()
    {
        base.Start();
        fixedCinemachine = transform.GetChild(0).gameObject;
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
                activated = true;
                camController.enabled = false;
            }
        }
        else
        {
            PositionPlayer();
        }
    }

    public void PositionPlayer()
    {
        playerMovement.enabled = false;
        playerBody.enabled = false;
        fixedCinemachine.SetActive(true);

    }

}
