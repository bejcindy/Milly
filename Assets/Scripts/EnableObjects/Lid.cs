using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using PixelCrushers.DialogueSystem;

public class Lid : LivableObject
{
    public bool lidMoving;
    public bool lidOpen;
    public int useCount;
    public Vector3 targetRot;
    Quaternion openRotation;
    Quaternion closeRotation;
    DialogueSystemTrigger lidDialogue;

    public LivableObject controlledContainer;
    public EventReference lidSound;
    bool iconHidden;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (lidOpen)
        {
            openRotation = transform.localRotation;
            closeRotation = Quaternion.Euler(targetRot);
        }
        else
        {
            openRotation = Quaternion.Euler(targetRot);
            closeRotation = transform.localRotation;
        }
    }

    // Update is called once per frame
    protected override void Update()
    { 
        base.Update();
        if (interactable && !lidMoving)
        {
            LidControl();
        }
        else
        {
            if (activated)
                gameObject.layer = 17;
            else
                gameObject.layer = 0;
            if (!iconHidden)
            {
                playerHolding.lidObj = null;
                iconHidden = true;
            }
        }

        if (!lidOpen || lidMoving)
        {
            if(controlledContainer != null)
            {
                controlledContainer.interactable = false;
                controlledContainer.enabled = false;
                BoxCollider containerTrigger = controlledContainer.GetComponent<BoxCollider>();
                if(containerTrigger != null)
                {
                    if (containerTrigger.isTrigger)
                        containerTrigger.enabled = false;
                }
            }
        }
        else
        {
            if (controlledContainer != null)
            {
                controlledContainer.enabled = true;
                BoxCollider containerTrigger = controlledContainer.GetComponent<BoxCollider>();
                if (containerTrigger != null)
                {
                    if (containerTrigger.isTrigger)
                        containerTrigger.enabled = true;
                }
            }
        }
    }

    void LidControl()
    {
        gameObject.layer = 9;

        if (Input.GetMouseButton(0))
        {
            float verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime;
            if (lidOpen)
            {
                if (verticalInput < 0)
                {
                    StartCoroutine(LerpRotation(closeRotation, 1f));
                }
                playerHolding.dragAnimDirection = "Down";
            }
            else
            {
                if (verticalInput > 0)
                {
                    StartCoroutine(LerpRotation(openRotation, 1f));
                }
                playerHolding.dragAnimDirection = "Up";
            }

            playerHolding.dragAnimDirection = "UpDown";
        }
        else
        {
            playerHolding.lidObj = transform.GetChild(0).gameObject;
            iconHidden = false;
        }

    }


    IEnumerator LerpRotation(Quaternion endValue, float duration)
    {
        activated = true;
        if (!lidMoving)
        {
            useCount++;
            if (useCount == 5)
                DialogueManager.StartConversation("Bathroom/ToiletSeat");
        }

        lidMoving = true;
        float time = 0;
        Quaternion startValue = transform.localRotation;
        while (time < duration)
        {
            transform.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        FMODUnity.RuntimeManager.PlayOneShot(lidSound, transform.position);
        transform.localRotation = endValue;
        lidMoving = false;

        if(endValue == openRotation)
        {
            lidOpen = true;
        }
        else
        {
            lidOpen = false;
        }
        
    }
}
