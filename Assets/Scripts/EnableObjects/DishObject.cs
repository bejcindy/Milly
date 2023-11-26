using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishObject : LivableObject
{
    public bool inHand;
    PlayerHolding playerHolding;

    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
    }

    protected override void Update()
    {
        base.Update();
        if(interactable && !inHand)
        {
            if (Input.GetMouseButtonDown(0) && playerHolding.GetLeftHand())
                inHand = true;
        }
    }
}
