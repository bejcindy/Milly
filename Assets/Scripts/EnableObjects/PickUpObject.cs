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

    public GameObject uiHint;
    public Sprite leftMouse;
    public Sprite rightMouse;


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
                if (playerHolding.GetLeftHand())
                {
                    uiHint.GetComponent<SpriteRenderer>().sprite = leftMouse;
                    uiHint.SetActive(true);
                    if (Input.GetMouseButtonDown(0))
                    {
                        uiHint.SetActive(false);
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

                if (playerHolding.GetRightHand())
                {
                    uiHint.GetComponent<SpriteRenderer>().sprite = rightMouse;
                    uiHint.SetActive(true);
                    if (Input.GetMouseButtonDown(1))
                    {
                        uiHint.SetActive(false);
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
        else if (!interactable || inHand)
        {
            uiHint.SetActive(false);
        }
    }
}
