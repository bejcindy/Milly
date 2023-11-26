using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PixelCrushers.DialogueSystem;

public class FixedCameraObject : LivableObject
{
    public bool dialogueBound;
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

    protected DialogueSystemTrigger dialogue;
    PlayerCam camController;
    PlayerMovement playerMovement;
    protected PlayerHolding playerHolding;
    public Renderer playerBody;

    protected bool iconHidden;
    protected bool isPizzaBox;

    protected bool movingCam;
    public float camXAxisSpeed;
    public float camYAxisSpeed;

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
                else if (gameObject.name.Contains("catbox"))
                {
                    playerHolding.catboxObj = gameObject;
                    DataHolder.ShowHint("<b>F</b> Check");
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
                else if (gameObject.name.Contains("catbox"))
                {
                    playerHolding.catboxObj = null;
                    DataHolder.HideHint("<b>F</b> Check");
                    iconHidden = true;
                }
            }
        }
        else
        {
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
                else if (gameObject.name.Contains("catbox"))
                {
                    playerHolding.catboxObj = null;
                    DataHolder.HideHint("<b>F</b> Check");
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

        if (!dialogueBound || (dialogueBound && !playerHolding.inDialogue))
            QuitInteraction();
    }

    public void QuitInteraction()
    {
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
                    TurnOnCamera();
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

        if (isPizzaBox)
        {
            player.GetComponent<PlayerLeftHand>().inPizzaBox = true;
        }

    }


    protected void PositionPlayer()
    {
        if (!moveCam)
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
            }
        }
        if (doubleSided)
        {
            if (onLeft)
                fixedCamera.m_Priority = 9;
            else if (onRight)
                otherCamera.m_Priority = 9;
        }
        else
            fixedCamera.m_Priority = 9;

        playerCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 200;
        playerCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 200;
        yield return new WaitForSeconds(2f);

        if (moveCam)
        {
            fixedCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = 0;
            fixedCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = 0;
        }

        camController.enabled = true;
        playerMovement.enabled = true;
        positionFixed = false;
        playerHolding.positionFixedWithMouse = false;
    }
}
