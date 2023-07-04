using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : LivableObject
{
    public PlayerHolding playerHolding;
    Rigidbody rb;

    public Transform holdingPos;
    public string pickUpText;
    DisplayText displayText;

    protected override void Start()
    {
        base.Start();
        holdingPos = Camera.main.transform;
        playerHolding = player.GetComponent<PlayerHolding>();
        rb = GetComponent<Rigidbody>();
        displayText = GameObject.Find("Thought").GetComponent<DisplayText>();
    }
    protected override void Update()
    {
        base.Update();
        DetectEnable();
    }
    private void DetectEnable()
    {
        if (interactable)
        {
            if (!playerHolding.isHolding)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //activate if not yet
                    if (!activated && matColorVal > 0)
                    {
                        activated = true;
                    }
                    //set player side
                    playerHolding.isHolding = true;
                    playerHolding.holdingObj = transform;

                    displayText.txt = pickUpText;
                    displayText.StartCoroutine("ShowText");

                    //move obj position and disable rb
                    transform.SetParent(holdingPos);
                    transform.localPosition = playerHolding.holdingPosition;
                    rb.isKinematic = true;
                }
                
            }
        }
    }
}
