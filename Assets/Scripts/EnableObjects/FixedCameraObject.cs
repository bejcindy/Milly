using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PixelCrushers.DialogueSystem;
using FMODUnity;
using VInspector;

public class FixedCameraObject : LivableObject
{
    [Foldout("Fixed Camera")]
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

    PlayerMovement playerMovement;

    protected bool iconHidden;
    protected bool isPizzaBox;
    public bool nearPlayer;

    protected bool movingCam;
    public float camXAxisSpeed;
    public float camYAxisSpeed;

    string sitSound = "event:/Player/Player_Sit";

    protected override void Start()
    {
        base.Start();
        playerMovement = ReferenceTool.playerMovement;
        playerCamera = ReferenceTool.playerCinemachine;
        camBrain = ReferenceTool.playerBrain;
        if (transform.parent != null)
        {
            if (transform.parent.name.Contains("pizza"))
                isPizzaBox = true;
        }

        if(interactKey == KeyCode.F)
        {
            quitKey = KeyCode.F;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (nearPlayer && checkVisible)
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
                    DataHolder.HideHint(DataHolder.hints.standHint);
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
                else if((!dialogueBound || (dialogueBound && !playerHolding.inDialogue)) && !isPizzaBox && positionFixed)
                {
                    if(dialogueBound)
                    playerHolding.sitObj = null;
                    DataHolder.HideHint(DataHolder.hints.sitHint);
                    DataHolder.ShowHint(DataHolder.hints.standHint);
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
                    DataHolder.HideHint(DataHolder.hints.standHint);
                    iconHidden = true;
                }
            }
        }

        if ((!dialogueBound || (dialogueBound && !playerHolding.inDialogue)) && !isPizzaBox && positionFixed )
            QuitInteraction();
        else if (isPizzaBox && positionFixed)
        {
            if (!GetComponent<PizzaLid>().myBox.movingPizza)
            {
                QuitInteraction();
            }
        }
    }

    public void QuitInteraction()
    {
        if (positionFixed && !camBrain.IsBlending)
        {
            if(interactKey == KeyCode.F)
            {
                if (Input.GetKeyDown(quitKey))
                {
                    QuitAction();
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    StartCoroutine(UnfixPlayer());
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                {
                    QuitAction();
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    StartCoroutine(UnfixPlayer());
                }
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
                {
                    TurnOnCamera();
                }

            }
            else
            {
                if (Input.GetKeyDown(interactKey))
                {
                    TurnOnCamera();
                }

            }
        }
    }

    public void TurnOnCamera()
    {
        isInteracting = true;
        if (!gameObject.name.Contains("apt_call"))
            RuntimeManager.PlayOneShot(sitSound, player.transform.position);

        ReferenceTool.playerPOV.m_VerticalAxis.m_MaxSpeed = 0;
        ReferenceTool.playerPOV.m_HorizontalAxis.m_MaxSpeed = 0;
        playerMovement.enabled = false;
        uiHint.SetActive(false);
        activated = true;



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
        


        if (showMouse)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerHolding.positionFixedWithMouse = true;
        }



        if (isPizzaBox)
        {
            playerLeftHand.inPizzaBox = true;
        }

        Invoke(nameof(SetInteracting), 1f);

    }

    void SetInteracting()
    {
        positionFixed = true;
    }



    protected IEnumerator UnfixPlayer()
    {
        playerCamera.m_Priority = 10;

        if (doubleSided)
        {
            if (onLeft)
                fixedCamera.m_Priority = 9;
            else if (onRight)
                otherCamera.m_Priority = 9;
        }
        else
            fixedCamera.m_Priority = 9;

        ReferenceTool.playerPOV.m_VerticalAxis.m_MaxSpeed = 200;
        ReferenceTool.playerPOV.m_HorizontalAxis.m_MaxSpeed = 200;
        yield return new WaitForSeconds(2f);

        if (moveCam)
        {
            fixedCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = 0;
            fixedCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = 0;
        }


        playerMovement.enabled = true;
        positionFixed = false;
        isInteracting = false;
        playerHolding.positionFixedWithMouse = false;

    }

    protected virtual void QuitAction()
    {

    }

    public void ManualQuit()
    {
        StartCoroutine(UnfixPlayer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            nearPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            nearPlayer = false;
    }
}
