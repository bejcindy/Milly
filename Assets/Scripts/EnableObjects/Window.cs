using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : LivableObject
{
    public bool isInteracting;
    public bool leftHandInteraction;
    public bool rightHandInteraction;
    PlayerHolding playerHolding;


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
            if (!isInteracting)
            {
                if (playerHolding.GetLeftHand())
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        isInteracting = true;
                        leftHandInteraction = true;
                        activated = true;
                    }
                }
                if (playerHolding.GetRightHand())
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        isInteracting = true;
                        rightHandInteraction = true;
                        activated = true;
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
                if (rightHandInteraction)
                {
                    if (Input.GetMouseButtonUp(1))
                    {
                        rightHandInteraction = false;
                        isInteracting = false;
                    }
                }
                UseWindow();
            }

        }
        else
        {
            isInteracting = false;
        }

    }


    public void UseWindow()
    {
        float verticalInput = Input.GetAxisRaw("Mouse Y");
        Debug.Log(verticalInput);
        if(verticalInput > 0)
        {
            if(transform.localPosition.y < 0.6f)
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
