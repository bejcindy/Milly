using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TrashLid : LivableObject
{
    public float rotateSpeed;
    public bool interacting;
    public bool fixedPos;
    public PlayerHolding playerHolding;

    //public GameObject leftHandUI;
    //public GameObject rightHandUI;
    GameObject iconPos;
    bool iconHidden;
    PlayerLeftHand playerLeftHand;

    public EventReference openSound, closeSound;
    bool openPlayed, closePlayed;

    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
        iconPos = transform.GetChild(0).gameObject;
        playerLeftHand = player.GetComponent<PlayerLeftHand>();
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
                //leftHandUI.SetActive(true);
                
                if (Input.GetMouseButton(0))
                {
                    //leftHandUI.SetActive(false);
                    activated = true;
                    interacting = true;
                    if (verticalInput < 0)
                    {
                        RotateLid(0);
                        if (!closePlayed && !closeSound.IsNull)
                        {
                            RuntimeManager.PlayOneShot(closeSound, transform.position);
                            closePlayed = true;
                        }
                    }
                    else if (verticalInput > 0)
                    {
                        RotateLid(270);
                        if (!openPlayed && !openSound.IsNull)
                        {
                            RuntimeManager.PlayOneShot(openSound, transform.position);
                            closePlayed = true;
                        }
                    }
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
                    //leftHandUI.SetActive(false);
                    activated = true;
                    interacting = true;
                    playerLeftHand.bypassThrow = true;
                    if (verticalInput < 0)
                    {
                        RotateLid(0);
                        if (!closePlayed && !closeSound.IsNull)
                        {
                            RuntimeManager.PlayOneShot(closeSound, transform.position);
                            closePlayed = true;
                        }
                    }
                    else if (verticalInput > 0)
                    {
                        RotateLid(270);
                    }
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

        if (interacting)
        {
            if (!iconHidden)
            {
                playerHolding.lidObj = null;
                iconHidden = true;
            }
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
