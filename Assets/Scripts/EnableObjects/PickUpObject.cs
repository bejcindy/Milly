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
    public bool selected;

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
            playerHolding.AddInteractable(this);
            if (!playerHolding.fullHand)
            {
                if (playerHolding.GetLeftHand())
                {
                    uiHint.GetComponent<SpriteRenderer>().sprite = leftMouse;
                    if (Input.GetMouseButtonDown(0) && selected)
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
            playerHolding.RemoveInteractable(this);
            selected = false;
            HideUI();
        }


        if (selected)
        {
            ShowUI();
        }
        else
        {
            HideUI();
        }
    }

    public void ShowUI()
    {
        uiHint.SetActive(true);
    }

    public void HideUI()
    {
        uiHint.SetActive(false);
    }
}
