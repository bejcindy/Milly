using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using VInspector;

public enum HandObjectType
{
    DRINK,
    TRASH,
    CIGARETTE,
    DISH,
    CHOPSTICKS,
    DOUBLE,
    PLACEMENT,
    CATFOOD,
    FOOD
}
public class PickUpObject : LivableObject
{
    protected Rigidbody rb;

    [Foldout("Pickup")]
    public HandObjectType objType;
    public Vector3 targetRot;
    public bool inHand;
    public bool selected;
    public bool thrown;
    public bool dumped;
    public bool freezeRotation;
    public float throwCD;
    public bool thrownByPlayer;


    DialogueSystemTrigger dialogue;
    public string pickUpEventName;
    public string throwEventName;

    public EventReference pickUpSound, collideSound;
    bool canPlayCollideSF;
    protected override void Start()
    {
        base.Start();
        pickType = true;
        rb = GetComponent<Rigidbody>();
        throwCD = 1f;
        dialogue = GetComponent<DialogueSystemTrigger>();
    }
    protected override void Update()
    {
        base.Update();
        if (!MainTattooMenu.tatMenuOn)
        {
            DetectEnable();
        }
        else
        {
            selected = false;
        }

        if (selected && !thrown)
            gameObject.layer = 9;
        else if (inHand)
            gameObject.layer = 7;
        else if (activated)
            gameObject.layer = 17;
        else
            gameObject.layer = 0;


        if (dumped)
        {
            minDist = 0;
            selected = false;
            rb.mass = 10;
            rb.velocity = Vector3.zero;
            playerHolding.RemoveInteractable(gameObject);
            if (activated)
            {
                gameObject.layer = 17;
            }
            else
            {
                gameObject.layer = 0;
            }
            this.enabled = false;
        }

    }
    private void DetectEnable()
    {
        if (interactable && !inHand && !thrown)
        {
            if (playerHolding.GetLeftHand())
            {
                if (!StartSequence.noControl||overrideStartSequence)
                {
                    playerHolding.AddInteractable(gameObject);
                }

                if (Input.GetMouseButtonDown(0) && selected)
                {
                    if (!activated && matColorVal > 0)
                    {
                        activated = true;
                    }
                    canPlayCollideSF = true;
                    rb.isKinematic = true;
                    if (!pickUpSound.IsNull)
                        RuntimeManager.PlayOneShot(pickUpSound, transform.position);
                    playerHolding.OccupyLeft(transform);
                    inHand = true;
                    //FMODUnity.RuntimeManager.PlayOneShot(pickUpEventName, player.transform.position);
                }



            }
            else
            {
                selected = false;
            }
        }
        else if (!interactable || inHand)
        {
            if (playerHolding.CheckInteractable(gameObject))
                playerHolding.RemoveInteractable(gameObject);
            selected = false;
        }

        if(interactable && !inHand && thrown)
        {
            playerHolding.midAirKickable = gameObject;
        }




        if (thrown)
        {
            ThrowCoolDown();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collideSound.IsNull && DataHolder.canMakeSound && !inHand && canPlayCollideSF)
        {
            RuntimeManager.PlayOneShot(collideSound, transform.position);
            canPlayCollideSF = false;
        }
        if (thrownByPlayer && collision.gameObject != player)
            thrownByPlayer = false;
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
