using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public enum HandObjectType
{
    DRINK,
    TRASH,
    CIGARETTE,
    DISH,
    CHOPSTICKS,
    DOUBLE,
    PLACEMENT,
    CATFOOD
}
public class PickUpObject : LivableObject
{
    public HandObjectType objType;
    public Vector3 targetRot;
    protected PlayerHolding playerHolding;
    protected Rigidbody rb;
    public bool inHand;
    public bool selected;
    public bool thrown;
    public bool cigarette;
    public bool freezeRotation;
    public float throwCD;
    public bool thrownByPlayer;

    [Header("Type")]
    public bool npcBound;
    public bool foodObj;

    DialogueSystemTrigger dialogue;
    public string pickUpEventName;
    public string throwEventName;

    public EventReference pickUpSound, collideSound;
    bool canPlayCollideSF;
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
            if (playerHolding.GetLeftHand())
            {
                if (!player.GetComponent<PlayerLeftHand>().inPizzaBox && (!StartSequence.noControl||overrideStartSequence))
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
        }
        else if (!interactable || inHand)
        {
            if (playerHolding.CheckInteractable(gameObject))
                playerHolding.RemoveInteractable(gameObject);
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
