using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PixelCrushers.DialogueSystem;

public class FixedCameraObject : LivableObject
{
    [SerializeField] KeyCode interactKey;
    [SerializeField] public KeyCode quitKey;
    [SerializeField] public bool isInteracting;
    [SerializeField] public bool positionFixed;
    public bool mouseActivate;
    [SerializeField] protected bool showMouse;
    [SerializeField] protected bool moveCam;


    [SerializeField] protected bool doubleSided;
    [SerializeField] public bool onLeft;
    [SerializeField] public bool onRight;

    [SerializeField] protected CinemachineVirtualCamera fixedCamera;
    [SerializeField] protected CinemachineVirtualCamera otherCamera;
    [SerializeField] protected CinemachineVirtualCamera playerCamera;
    CinemachineBrain camBrain;

    [SerializeField] public GameObject uiHint;

    DialogueSystemTrigger dialogue;
    PlayerCam camController;
    PlayerMovement playerMovement;
    protected PlayerHolding playerHolding;
    public Renderer playerBody;

    bool iconHidden;
    bool isPizzaBox;
    protected override void Start()
    {
        base.Start();
        playerMovement = player.GetComponent<PlayerMovement>();
        camController = player.GetComponent<PlayerCam>();
        playerBody = player.GetChild(0).GetComponent<Renderer>();
        playerCamera = GameObject.Find("PlayerCinemachine").GetComponent<CinemachineVirtualCamera>();
        playerHolding = player.GetComponent<PlayerHolding>();
        camBrain = Camera.main.GetComponent<CinemachineBrain>();
        if (transform.parent != null)
        {
            if (transform.parent.name.Contains("pizza"))
                isPizzaBox = true;
        }
    }



    protected override void Update()
    {
        base.Update();
        if (interactable)
        {
            if (!isInteracting)
            {
                //uiHint.SetActive(true);
                if (gameObject.name.Contains("apt_call"))
                {
                    playerHolding.clickableObj = gameObject;
                    iconHidden = false;
                }
                else if (isPizzaBox)
                {
                    playerHolding.lidObj = gameObject;
                    iconHidden = false;
                }
                else
                {
                    playerHolding.sitObj = uiHint;
                    DataHolder.ShowHint(DataHolder.hints.sitHint);
                    iconHidden = false;
                }


                TriggerInteraction();
            }
            else
            {
                if (gameObject.name.Contains("apt_call"))
                {
                    playerHolding.clickableObj = null;
                    iconHidden = true;
                }
            }
        }
        else
        {
            //gameObject.layer = 0;
            //uiHint.SetActive(false);
            if (!iconHidden)
            {
                if (gameObject.name.Contains("apt_call"))
                {
                    playerHolding.clickableObj = null;
                    iconHidden = true;
                }
                else if (isPizzaBox)
                {
                    playerHolding.lidObj = null;
                    iconHidden = true;
                }
                else
                {
                    playerHolding.sitObj = null;
                    DataHolder.HideHint(DataHolder.hints.sitHint);
                    iconHidden = true;
                }
            }
        }

        if (positionFixed && !camBrain.IsBlending)
        {
            if (Input.GetKeyDown(quitKey) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                StartCoroutine(UnfixPlayer());
            }
        }



    }


    public virtual void TriggerInteraction()
    {
        if (!camBrain.IsBlending)
        {
            if (mouseActivate)
            {
                if (Input.GetMouseButtonDown(0))
                    TurnOnCamera();
            }
            else
            {
                if (Input.GetKeyDown(interactKey))
                {
                    //gameObject.layer = 0;
                    TurnOnCamera();
                }
            }
        }


    }

    public void TurnOnCamera()
    {
        playerBody.enabled = false;
        uiHint.SetActive(false);
        activated = true;
        positionFixed = true;
        if (showMouse)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerHolding.positionFixedWithMouse = true;
        }
        PositionPlayer();

        if(isPizzaBox)
        {
            player.GetComponent<PlayerLeftHand>().inPizzaBox = true;
            player.GetComponent<PlayerRightHand>().inPizzaBox = true;
        }

    }


    protected void PositionPlayer()
    {
        playerMovement.enabled = false;
        playerBody.enabled = false;
        playerCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
        playerCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
        if (doubleSided)
        {
            if (onLeft)
            {
                fixedCamera.m_Priority = 10;
            }
            else if (onRight)
            {
                otherCamera.m_Priority = 10;
            }
            else
            {
                fixedCamera.m_Priority = 10;
            }
        }
        else
        {
            fixedCamera.m_Priority = 10;
        }
        playerCamera.m_Priority = 9;
        isInteracting = true;

        if (gameObject.name == "Kelvin_Rock")
        {
            KelvinRockChair();
        }
    }


    protected IEnumerator UnfixPlayer()
    {
        playerCamera.m_Priority = 10;
        isInteracting = false;
        if (transform.parent != null)
        {
            if (transform.parent.name.Contains("pizza"))
            {
                player.GetComponent<PlayerLeftHand>().inPizzaBox = false;
                player.GetComponent<PlayerRightHand>().inPizzaBox = false;
            }
        }
        if (doubleSided)
        {
            if (onLeft)
            {
                fixedCamera.m_Priority = 9;
            }
            else if (onRight)
            {
                otherCamera.m_Priority = 9;
            }
        }
        else
        {
            fixedCamera.m_Priority = 9;
        }

        playerCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 200;
        playerCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 200;
        yield return new WaitForSeconds(2f);

        if (moveCam)
        {
            fixedCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = 0;
            fixedCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = 0;
        }
        //playerBody.enabled = true;
        camController.enabled = true;
        playerMovement.enabled = true;
        positionFixed = false;
        playerHolding.positionFixedWithMouse = false;

    }

    public void KelvinRockChair()
    {
        Kelvin kelv = GameObject.Find("Kelvin").GetComponent<Kelvin>();
        DialogueLua.SetVariable("NPC/Kelvin/PlayerAtRock", true);
        kelv.npcActivated = true;
    }
}
