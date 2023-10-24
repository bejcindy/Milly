using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TableObject : LivableObject
{
    public TableController tableController;
    public bool inHand;
    public bool thrown;
    public bool selected;
    PlayerHolding playerHolding;
    Vector3 startingPos;

    protected override void Start()
    {
        base.Start();
        playerHolding = GetComponent<PlayerHolding>();
        startingPos = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        if (playerHolding.atTable && tableController.tableControlOn)
        {
            DetectEnable();
        }
        else
        {
            selected = false;
            if (activated)
                gameObject.layer = 17;
            else
                gameObject.layer = 0;
        }
    }



    private void DetectEnable()
    {
        if (interactable && !inHand)
        {
            if (playerHolding.GetLeftHand())
            {
                if (!StartSequence.noControl)
                {
                    playerHolding.AddInteractable(gameObject);
                }

                if (Input.GetMouseButtonDown(0) && selected)
                {
                    if (!activated && matColorVal > 0)
                    {
                        activated = true;
                    }
                    playerHolding.OccupyLeft(transform);
                    inHand = true;
                }
            }
        }

        else if (!interactable || inHand)
        {
            selected = false;
            if (playerHolding.CheckInteractable(gameObject))
                playerHolding.RemoveInteractable(gameObject);
        }



        if (selected)
            gameObject.layer = 9;
        else if (inHand)
            gameObject.layer = 7;
        else if (activated)
            gameObject.layer = 17;
        else
            gameObject.layer = 0;


    }

}
