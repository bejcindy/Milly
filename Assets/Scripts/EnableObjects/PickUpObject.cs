using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : LivableObject
{
    public PlayerHolding playerHolding;
    protected Rigidbody rb;
    public string pickUpText;
    DisplayText displayText;
    public bool inHand;
    public bool selected;
    public bool thrown;
    public float throwCD;

    public GameObject uiHint;
    public Sprite leftMouse;
    public Sprite rightMouse;


    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
        rb = GetComponent<Rigidbody>();
        displayText = GameObject.Find("Thought").GetComponent<DisplayText>();
        throwCD = 1f;
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
            if (!player.GetComponent <PlayerLeftHand>().inPizzaBox && !player.GetComponent<PlayerRightHand>().inPizzaBox)
            {
                playerHolding.AddInteractable(this);
            }
            if (!playerHolding.fullHand)
            {
                if (playerHolding.GetLeftHand())
                {
                    uiHint.GetComponent<SpriteRenderer>().sprite = leftMouse;
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
                    uiHint.GetComponent<SpriteRenderer>().sprite = rightMouse;
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
