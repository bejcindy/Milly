using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : LivableObject
{
    public PlayerHolding playerHolding;
    public MakeAlive activateLogic;
    Rigidbody rb;

    public Transform holdingPos;


    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
        activateLogic = GetComponent<MakeAlive>();
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
                    if (!activateLogic.activated)
                    {
                        activateLogic.Activate();
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
