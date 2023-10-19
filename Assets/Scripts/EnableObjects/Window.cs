using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : LivableObject
{
    public bool isInteracting;
    public bool leftHandInteraction;
    public bool rightHandInteraction;
    public GameObject leftHandUI;
    public GameObject rightHandUI;
    PlayerHolding playerHolding;
    bool iconHidden;

    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
    }

    protected override void Update()
    {
        base.Update();
        if (interactable)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.layer = 9;
            }

            if (!isInteracting)
            {
                if (playerHolding.GetLeftHand())
                {
                    //leftHandUI.SetActive(true);
                    gameObject.layer = 9;
                    if (Input.GetMouseButtonDown(0))
                    {
                        isInteracting = true;
                        leftHandInteraction = true;
                        activated = true;
                    }
                    else
                    {
                        playerHolding.lidObj = leftHandUI;
                        iconHidden = false;
                    }
                }
            }
            else
            {

                if (leftHandInteraction)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        leftHandInteraction = false;
                        isInteracting=false;
                    }
                }
                if (!iconHidden)
                {
                    playerHolding.lidObj = null;
                    iconHidden = true;
                }
                UseWindow();
            }

        }
        else
        {
            foreach(Transform child in transform)
            {
                child.gameObject.layer = 0;
            }

            isInteracting = false;
            gameObject.layer = 0;
            if (!iconHidden)
            {
                playerHolding.lidObj = null;
                iconHidden = true;
            }
        }

    }


    public void UseWindow()
    {
        activated = true;
        float verticalInput = Input.GetAxisRaw("Mouse Y");
        Debug.Log(verticalInput);
        if(verticalInput > 0)
        {
            if(transform.localPosition.y < 0.57f)
            {
                transform.localPosition += Vector3.up * verticalInput * Time.deltaTime * 10f;
            }
        }
        else if(verticalInput < 0)
        {
            if(transform.localPosition.y > -0.55f)
            {
                transform.localPosition += Vector3.up * verticalInput * Time.deltaTime * 10f;
            }
        
        }
    }
}
