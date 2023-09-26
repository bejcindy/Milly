using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class IzakayaDoor : LivableObject
{
    public bool doorInteractable;
    public bool isInteracting;
    public bool leftHandOpen;
    public bool rightHandOpen;
    public bool doorOpen;
    public bool doorOpening;
    public Vector3 doorOpenPos;

    DialogueSystemTrigger dialogue;
    PlayerHolding playerHolding;
    FixedCameraObject cameraControl;
    Vector3 doorStartPos;

    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
        doorStartPos = transform.parent.transform.localPosition;
        //dialogue = GetComponent<DialogueSystemTrigger>();
    }
    protected override void Update()
    {
        base.Update();
        DoorControl();

    }

    void DoorControl()
    {
        if (doorInteractable)
        {
            if (playerHolding.GetLeftHand())
            {
                if (Input.GetMouseButton(0))
                {
                    activated = true;
                    leftHandOpen = true;
                    isInteracting = true;
                }
            }
        }

        if (isInteracting)
        {
            float horizontalInput = Input.GetAxis("Mouse X") * Time.deltaTime * 75;
            if (!doorOpening)
            {
                if (!doorOpen)
                {
                    if (horizontalInput < 0)
                    {
                        StartCoroutine(LerpPosition(doorOpenPos, 1f));
                    }
                }
                else
                {
                    if (horizontalInput > 0)
                    {
                        StartCoroutine(LerpPosition(doorStartPos, 1f));
                    }
                }
            }
        }



        if (leftHandOpen)
        {
            if (Input.GetMouseButtonUp(0))
            {
                isInteracting = false;
            }
        }
        

        if (transform.parent.localPosition == doorOpenPos)
        {
            doorOpen = true;
        }
        else
        {
            doorOpen = false;
        }
    }

    public void OpenDoor()
    {
        Debug.Log("NPC opening door");
        if (!doorOpen && !doorOpening)
        {
            StartCoroutine(LerpPosition(doorOpenPos, 1f));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (checkVisible)
                doorInteractable = true;
            else
                doorInteractable = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorInteractable = false;
        }
    }

    void TriggerConversation()
    {
        if (interactable)
        {
            if (playerHolding.GetLeftHand())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    dialogue.enabled = true;
                }
            }
            else if (playerHolding.GetRightHand())
            {
                if (Input.GetMouseButtonDown(1))
                {
                    dialogue.enabled = true;
                }
            }
        }
        else
        {
            dialogue.enabled = false;
        }
    }
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        doorOpening = true;
        float time = 0;
        Vector3 startPosition = transform.parent.transform.localPosition;
        while (time < duration)
        {
            transform.parent.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.parent.transform.localPosition = targetPosition;
        doorOpening = false;
    }
}
