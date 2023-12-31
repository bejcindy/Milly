using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Cinemachine;
using PixelCrushers.DialogueSystem;

public class Window : LivableObject
{
    public bool isInteracting;
    public GameObject leftHandUI;

    public bool windowOpen;
    public bool windowMoving;
    public bool cutsceneWindow;
    public CinemachineVirtualCamera windowCam;
    public Zayne zayne;
    public Vector3 openPos;
    public Vector3 closePos;
    string openSound = "event:/Sound Effects/ObjectInteraction/Window/WindowOpen";
    string closeSound = "event:/Sound Effects/ObjectInteraction/Window/WindowClose";
    bool soundPlayed;
    bool iconHidden;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (interactable && ! windowMoving)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.layer = 9;
            }
            if (!isInteracting)
            {
                if (playerHolding.GetLeftHand())
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        isInteracting = true;
                        activated = true;
                    }
                    else
                    {
                        playerHolding.lidObj = leftHandUI;
                        iconHidden = false;
                    }
                }
            }
            else
            {

                if (Input.GetMouseButtonUp(0))
                {
                    isInteracting = false;
                }

                
                UseWindow();
            }
        }
        else
        {
            isInteracting = false;
            foreach (Transform child in transform)
            {
                if (!child.gameObject.name.Contains("glass"))
                {
                    if (activated)
                        child.gameObject.layer = 17;
                    else
                        child.gameObject.layer = 0;
                }

            }
        }
        if (!interactable)
        {
            if (!iconHidden)
            {
                playerHolding.lidObj = null;
                iconHidden = true;
            }
        }
    }

    public void UseWindow()
    {
        float verticalInput = Input.GetAxisRaw("Mouse Y");

        if(verticalInput > 0)
        {
            if (!windowOpen)
            {
                if (!soundPlayed)
                {
                    RuntimeManager.PlayOneShot(openSound, transform.position);
                    soundPlayed = true;
                }


                StartCoroutine(LerpPosition(openPos, 1f));
            }

            //if(transform.localPosition.y < 0.57f)
            //{
            //    transform.localPosition += Vector3.up * verticalInput * Time.deltaTime * 10f;
            //}
        }
        else if(verticalInput < 0)
        {
            if (windowOpen)
            {
                if (!soundPlayed)
                {
                    RuntimeManager.PlayOneShot(closeSound, transform.position);
                    soundPlayed = true;
                }

                StartCoroutine(LerpPosition(closePos, 1f));
            }

            //if(transform.localPosition.y > -0.55f)
            //{
            //    transform.localPosition += Vector3.up * verticalInput * Time.deltaTime * 10f;
            //}
        
        }
        playerHolding.dragAnimDirection = "UpDown";
    }

    public void SetWindowUsable()
    {
        overrideStartSequence = true;
    }

    public void SetWindowUnusable()
    {
        overrideStartSequence = false;
    }

    public void FinishCutScene()
    {
        windowCam.m_Priority = 0;
        cutsceneWindow = false;
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        windowMoving = true;


        float time = 0;
        Vector3 startPosition = transform.localPosition;
        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition;

        if (targetPosition == openPos)
        {
            windowOpen = true;
            if (cutsceneWindow)
            {
                ReferenceTool.playerMovement.enabled = false;
                zayne.ChangeMainQuestDialogue();
                windowCam.m_Priority = 11;
                minDist = 0;
            }
        }

        else
            windowOpen = false;

        windowMoving = false;
        soundPlayed = false;
    }
}
