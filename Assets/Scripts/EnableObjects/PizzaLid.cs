using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PizzaLid : FixedCameraObject
{

    public bool openLid;
    public bool lidMoving;
    public PizzaBox myBox;
    public Vector3 targetRot;
    Quaternion openRotation;
    Quaternion closeRotation;

    string openEventName = "event:/Sound Effects/ObjectInteraction/PizzaBox_Open";
    string closeEventName = "event:/Sound Effects/ObjectInteraction/PizzaBox_Close";
    public EventReference openSound, closeSound;
    FMOD.Studio.EventInstance openEvent, closeEvent;
    bool openPlayed, closePlayed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        isPizzaBox = true;
        openRotation = Quaternion.Euler(targetRot);
        closeRotation = transform.localRotation;
        dialogue = GetComponent<DialogueSystemTrigger>();
        openEvent = RuntimeManager.CreateInstance(openSound);
        closeEvent = RuntimeManager.CreateInstance(closeSound);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (interactable)
        {
            nearPlayer = true;
        }
        else
        {
            nearPlayer = false;
        }
        if (nearPlayer && !lidMoving)
        {
            if(!openLid) 
                gameObject.layer = 9;
            else
            {
                if (activated)
                    gameObject.layer = 17;
                else
                    gameObject.layer = 0;
            }
            if (Input.GetMouseButton(0))
            {
                float verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime;

                if (openLid)
                    playerHolding.dragAnimDirection = "Down";
                else
                    playerHolding.dragAnimDirection = "Up";

                if (verticalInput < 0 && openLid)
                {
                    openLid = false;
                    StartCoroutine(LerpRotation(closeRotation, 1f));

                    if (!closePlayed)
                    {
                        RuntimeManager.PlayOneShot(closeEventName);
                        openPlayed = false;
                        closePlayed = true;
                    }
                }
                else if (verticalInput > 0 && !openLid)
                {
                    openLid = true;
                    dialogue.enabled = true;
                    activated = true;
                    StartCoroutine(LerpRotation(openRotation, 1f));
                    TurnOnCamera();

                    if (!openPlayed)
                    {
                        RuntimeManager.PlayOneShot(openEventName);
                        closePlayed = false;
                        openPlayed = true;
                    }
                }
            }

            //if (openLid)
            //{
            //    playerHolding.lidObj = null;
            //    DataHolder.ShowHint("<b>F/Move</b> Leave");
            //    iconHidden = true;
            //}
            //else
            //{
            //    DataHolder.HideHint("<b>F/Move</b> Leave");
            //}
        }
        else
        {
            if (activated)
                gameObject.layer = 17;
            else
                gameObject.layer = 0;
            //if (!iconHidden)
            //{
            //    playerHolding.lidObj = null;
            //    DataHolder.HideHint("<b>F/Move</b> Leave");
            //    iconHidden = true;
            //}
        }

        if (isInteracting && openLid)
        {
            playerLeftHand.inPizzaBox = true;
        }
        else if(!isInteracting)
        {
            playerLeftHand.inPizzaBox = false;
            if (openLid && !lidMoving)
            {
                openLid = false;
                StartCoroutine(LerpRotation(closeRotation, 1f));
            }
        }
    }

    protected override void QuitAction()
    {
        if(!lidMoving)
            StartCoroutine(LerpRotation(closeRotation, 1f));
        if (!closePlayed)
        {
            RuntimeManager.PlayOneShot(closeEventName);
            openPlayed = false;
            closePlayed = true;
        }
    }

    IEnumerator LerpRotation(Quaternion endValue, float duration)
    {

        lidMoving = true;
        float time = 0;
        Quaternion startValue = transform.localRotation;
        while (time < duration)
        {
            transform.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = endValue;
        lidMoving = false;
    }
}
