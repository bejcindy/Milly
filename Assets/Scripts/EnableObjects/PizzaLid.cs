using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PizzaLid : FixedCameraObject
{
    public bool interacting;
    public bool openLid;
    public float rotateSpeed;
    public bool quitInteraction;
    public bool coolDown;

    public float coolDownVal;
    public bool fixedPos;

    PlayerLeftHand playerLeftHand;
    public EventReference openSound, closeSound;
    FMOD.Studio.EventInstance openEvent, closeEvent;
    bool openPlayed, closePlayed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        isPizzaBox = true;
        dialogue = GetComponent<DialogueSystemTrigger>();
        playerLeftHand = player.GetComponent<PlayerLeftHand>();
        openEvent = RuntimeManager.CreateInstance(openSound);
        closeEvent = RuntimeManager.CreateInstance(closeSound);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (transform.eulerAngles.z == 0 || transform.eulerAngles.z >= 359 || transform.eulerAngles.z == 300)
            fixedPos = true;
        else
            fixedPos = false;

        if (interactable && !coolDown && playerHolding.GetLeftHand())
        {

            float verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime * 75;

            if (Input.GetMouseButton(0))
            {
                dialogue.enabled = true;
                activated = true;
                interacting = true;
                quitInteraction = false;
                if (verticalInput < 0)
                {
                    RotateLid(0);

                    if (!closePlayed && !closeSound.IsNull)
                    {
                        openEvent.start();
                        closeEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                        openPlayed = false;
                        closePlayed = true;
                    }
                }
                else if (verticalInput > 0)
                {
                    RotateLid(300);
                    TurnOnCamera();
                    isInteracting = true;

                    if (!openPlayed && !openSound.IsNull)
                    {
                        closeEvent.start();
                        openEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                        closePlayed = false;
                        openPlayed = true;
                    }
                }
            }

            if (openLid)
            {
                playerHolding.lidObj = null;
                iconHidden = true;
            }
        }
        else
        {
            if (!iconHidden)
            {
                playerHolding.lidObj = null;
                iconHidden = true;
            }
        }

        if (isInteracting && openLid)
        {
            playerLeftHand.inPizzaBox = true;
        }
        else
        {
            playerLeftHand.inPizzaBox = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            interacting = false;

        }

        if (!fixedPos && !interacting && !coolDown)
        {
            if (transform.eulerAngles.z < 310)
            {
                RotateLid(300);
            }
            else if (transform.eulerAngles.z != 360)
            {
                RotateLid(0);
            }
        }


        if (coolDown)
        {
            StopCoolDown();
            if(transform.eulerAngles.z < 359) 
                RotateLid(0);
        }
            

    }

    protected override void QuitAction()
    {
        openLid = false;
        coolDown = true;
        interacting = false;
    }

    void StopCoolDown()
    {
        if (coolDownVal > 0)
            coolDownVal -= Time.deltaTime;
        else
        {
            coolDown = false;
            coolDownVal = 2;
        }
    }


    void RotateLid(float zTargetAngle)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
            Mathf.LerpAngle(transform.eulerAngles.z, zTargetAngle, Time.deltaTime * rotateSpeed));
        if (zTargetAngle == 300)
            openLid = true;
        else
            openLid = false;
    }

}
