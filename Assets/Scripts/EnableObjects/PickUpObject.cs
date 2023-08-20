using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : LivableObject
{
    public PlayerHolding playerHolding;
    protected Rigidbody rb;
    public bool inHand;
    public bool selected;
    public bool thrown;
    public float throwCD;

    public bool npcBound;

    public GameObject leftUIHint;
    public GameObject rightUIHint;
    public GameObject uiHint;

    DialogueSystemTrigger dialogue;


    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
        rb = GetComponent<Rigidbody>();
        throwCD = 1f;
        dialogue = GetComponent<DialogueSystemTrigger>();
    }
    protected override void Update()
    {
        base.Update();
        DetectEnable();
    }
    private void DetectEnable()
    {
        if (interactable && !inHand && !thrown)
        {
            if (!npcBound)
            {
                if(dialogue!=null)
                    dialogue.enabled = true;
            }

            if (!player.GetComponent <PlayerLeftHand>().inPizzaBox && !player.GetComponent<PlayerRightHand>().inPizzaBox)
            {
                playerHolding.AddInteractable(this);
            }
            if (!playerHolding.fullHand)
            {
                if (playerHolding.GetLeftHand())
                {
                    
                    uiHint = leftUIHint;
                    if (Input.GetMouseButtonDown(0) && selected)
                    {
                        HideUI();
                        if (!activated && matColorVal > 0)
                        {
                            activated = true;
                        }

                        //displayText.txt = pickUpText;
                        //displayText.StartCoroutine("ShowText");
                        rb.isKinematic = true;
                        playerHolding.OccupyLeft(transform);
                        inHand = true;
                    }
                }

                if (playerHolding.GetRightHand())
                {
                    uiHint = rightUIHint;
                    if (Input.GetMouseButtonDown(1) && selected)
                    {
                        HideUI();
                        if (!activated && matColorVal > 0)
                        {
                            activated = true;
                        }
                        //displayText.txt = pickUpText;
                        //displayText.StartCoroutine("ShowText");
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

        //if(activated && npcBound)
        //{
        //    dialogue.enabled = true;
        //}


        if (selected && !thrown)
        {
            ShowUI();
            gameObject.layer = 9;
        }
        else
        {
            HideUI();
            gameObject.layer = 7;
        }

        if (thrown)
        {
            ThrowCoolDown();
        }
    }

    protected void ShowUI()
    {
        uiHint.SetActive(true);
    }

    protected void HideUI()
    {
        uiHint.SetActive(false);
        gameObject.layer = 7;
    }

    void ThrowCoolDown()
    {
        if(throwCD >0)
            throwCD -= Time.deltaTime;
        else
        {
            throwCD = 1f;
            thrown = false;
        }
    }
}
