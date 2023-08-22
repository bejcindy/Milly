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


            if (!playerHolding.fullHand)
            {
                if (!player.GetComponent<PlayerLeftHand>().inPizzaBox && !player.GetComponent<PlayerRightHand>().inPizzaBox)
                {
                    playerHolding.AddInteractable(this);
                }
                if (playerHolding.GetLeftHand())
                {
                    if (Input.GetMouseButtonDown(0) && selected)
                    {
                        if (!activated && matColorVal > 0)
                        {
                            activated = true;
                        }
                        rb.isKinematic = true;
                        playerHolding.OccupyLeft(transform);
                        inHand = true;
                    }
                }

                if (playerHolding.GetRightHand())
                {
                    if (Input.GetMouseButtonDown(1) && selected)
                    {
                        if (!activated && matColorVal > 0)
                        {
                            activated = true;
                        }
                        rb.isKinematic = true;
                        playerHolding.OccupyRight(transform);
                        inHand = true;
                    }
                }

            }
        }
        else if (!interactable || inHand)
        {
            if(playerHolding.CheckInteractable(this))
                playerHolding.RemoveInteractable(this);
            selected = false;
        }



        if (selected && !thrown)
        {
            gameObject.layer = 9;
        }
        else
        {
            gameObject.layer = 7;
        }

        if (thrown)
        {
            ThrowCoolDown();
        }
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
