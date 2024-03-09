using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TrashLid : LivableObject
{
    public float rotateSpeed;
    public bool interacting;
    public bool fixedPos;

    GameObject iconPos;
    bool iconHidden;
    public EventReference openSound, closeSound;
    bool openPlayed, closePlayed;
    FMOD.Studio.EventInstance openEvent, closeEvent;
    protected override void Start()
    {
        base.Start();
        iconPos = transform.GetChild(0).gameObject;
        openEvent = FMODUnity.RuntimeManager.CreateInstance(openSound);
        closeEvent= FMODUnity.RuntimeManager.CreateInstance(closeSound);
    }
    protected override void Update()
    {
        base.Update();
        if (transform.eulerAngles.z == 0 || transform.eulerAngles.z >= 359 || transform.eulerAngles.z == 270)
            fixedPos = true;
        else
            fixedPos = false;
        if (interactable)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime * 75;
            if (playerHolding.GetLeftHand())
            {
                if (Input.GetMouseButton(0))
                {
                    activated = true;
                    interacting = true;
                    if (verticalInput < 0)
                    {
                        RotateLid(0);
                        if (!closePlayed && !closeSound.IsNull)
                        {
                            //RuntimeManager.PlayOneShot(closeSound, transform.position);
                            openEvent.start();
                            closeEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                            openPlayed = false;
                            closePlayed = true;
                        }
                    }
                    else if (verticalInput > 0)
                    {
                        RotateLid(270);
                        if (!openPlayed && !openSound.IsNull)
                        {
                            closeEvent.start();
                            openEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                            closePlayed = false;
                            openPlayed = true;
                        }
                    }
                    playerHolding.dragAnimDirection = "UpDown";
                }
                else
                {
                    playerHolding.lidObj = iconPos;
                    openPlayed = false;
                    closePlayed = false;
                    iconHidden = false;
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    activated = true;
                    interacting = true;
                    if (!playerLeftHand.aiming)
                        playerLeftHand.bypassThrow = true;
                    if (verticalInput < 0)
                    {
                        if (!playerLeftHand.aiming)
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
                    }
                    else if (verticalInput > 0)
                    {
                        RotateLid(270);
                        if (!openPlayed && !openSound.IsNull)
                        {
                            closeEvent.start();
                            openEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                            closePlayed = false;
                            openPlayed = true;
                        }
                    }
                    playerHolding.dragAnimDirection = "UpDown";
                }
                else
                {
                    playerHolding.lidObj = iconPos;
                    iconHidden = false;
                    openPlayed = false;
                    closePlayed = false;
                    playerLeftHand.bypassThrow = false;
                }
            }

        }
        else
        {
            if (!iconHidden)
            {
                playerHolding.lidObj = null;
                iconHidden = true;
                playerLeftHand.bypassThrow = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            interacting = false;

        }

        if (!fixedPos && !interacting)
        {
            if (transform.eulerAngles.z < 290)
            {
                RotateLid(270);
            }
            else if (transform.eulerAngles.z != 360)
            {
                RotateLid(0);
            }
        }

    }


    void RotateLid(float zTargetAngle)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
            Mathf.LerpAngle(transform.eulerAngles.z, zTargetAngle, Time.deltaTime * rotateSpeed));
    }
}
