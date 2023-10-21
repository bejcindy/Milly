using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PizzaLid : LivableObject
{
    public bool interacting;
    public bool openLid;
    public float rotateSpeed;
    public bool quitInteraction;
    public bool coolDown;
    public bool isInteracting;

    public float coolDownVal;
    bool fixedPos;
    PlayerHolding playerHolding;
    FixedCameraObject camControl;
    DialogueSystemTrigger dialogue;
    PlayerLeftHand playerLeftHand;
    public EventReference openSound, closeSound;
    FMOD.Studio.EventInstance openEvent, closeEvent;
    bool openPlayed, closePlayed;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        camControl = GetComponent<FixedCameraObject>();
        playerHolding = player.GetComponent<PlayerHolding>();
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
        if (interactable && !coolDown)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime * 75;
            if (playerHolding.GetLeftHand())
            {
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
                        camControl.TurnOnCamera();
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
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    dialogue.enabled = true;
                    activated = true;
                    interacting = true;
                    quitInteraction = false;
                    playerLeftHand.bypassThrow = true;
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
                        camControl.TurnOnCamera();
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
                else
                {
                    playerLeftHand.bypassThrow = false;
                    openPlayed = false;
                    closePlayed = false;
                }

            }
            //if (playerHolding.GetRightHand())
            //{
            //    if (Input.GetMouseButton(1))
            //    {
            //        dialogue.enabled = true;
            //        activated = true;
            //        interacting = true;
            //        quitInteraction = false;
            //        if (verticalInput < 0)
            //        {
            //            RotateLid(0);
            //        }
            //        else if (verticalInput > 0)
            //        {
            //            RotateLid(300);
            //            camControl.TurnOnCamera();
            //            isInteracting = true;
            //        }
            //    }
            //}

        }

        if (isInteracting && openLid)
        {
            player.GetComponent<PlayerLeftHand>().inPizzaBox = true;
            player.GetComponent<PlayerRightHand>().inPizzaBox = true;
        }
        else
        {
            player.GetComponent<PlayerLeftHand>().inPizzaBox = false;
            player.GetComponent<PlayerRightHand>().inPizzaBox = false;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
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

        if (isInteracting && Input.GetKeyDown(camControl.quitKey))
        {
            isInteracting = false;
            openLid = false;
            coolDown = true;
            interacting = false;
        }


        if (coolDown)
        {
            StopCoolDown();
            if(transform.eulerAngles.z < 359) 
                RotateLid(0);
        }
            

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

    IEnumerator LerpRotation(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.localEulerAngles;
        while (time < duration)
        {
            transform.localEulerAngles = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localEulerAngles = targetPosition;
    }
}
