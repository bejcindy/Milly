using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : LivableObject
{
    public PlayerHolding playerHolding;
    Rigidbody rb;
    public string pickUpText;
    DisplayText displayText;
    public bool inHand;


    protected override void Start()
    {
        base.Start();
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
        if (interactable && !inHand)
        {
            if (!playerHolding.fullHand)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (playerHolding.GetLeftHand())
                    {
                        if (!activated && matColorVal > 0)
                        {
                            activated = true;
                        }

                        displayText.txt = pickUpText;
                        displayText.StartCoroutine("ShowText");
                        rb.isKinematic = true;
                        playerHolding.OccupyLeft(transform);
                        inHand = true;
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    if (playerHolding.GetRightHand())
                    {
                        if (!activated && matColorVal > 0)
                        {
                            activated = true;
                        }
                        displayText.txt = pickUpText;
                        displayText.StartCoroutine("ShowText");
                        rb.isKinematic = true;
                        playerHolding.OccupyRight(transform);
                        inHand = true;
                    }
                }

            }
        }
    }
}
