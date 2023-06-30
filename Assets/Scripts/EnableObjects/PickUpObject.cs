using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : LivableObject
{
    public PlayerHolding playerHolding;
    Rigidbody rb;

    public Transform holdingPos;


    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
        rb = GetComponent<Rigidbody>();
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

                    //move obj position and disable rb
                    transform.SetParent(holdingPos);
                    transform.localPosition = playerHolding.holdingPosition;
                    rb.isKinematic = true;
                }
                
            }
        }
    }
}
