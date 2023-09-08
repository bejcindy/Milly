using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class IzakayaDoor : LivableObject
{
    public bool isInteracting;
    public bool leftHandOpen;
    public bool rightHandOpen;
    public bool doorOpen;
    public bool doorOpenable;
    public Transform door;
    public Vector3 doorOpenPos;

    DialogueSystemTrigger dialogue;
    PlayerHolding playerHolding;
    FixedCameraObject cameraControl;
    Vector3 doorStartPos;

    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
        cameraControl = GetComponent<FixedCameraObject>();
        doorStartPos = door.transform.localPosition;
        dialogue = GetComponent<DialogueSystemTrigger>();
    }
    protected override void Update()
    {
        base.Update();
        if (doorOpenable)
        {
            OpenDoor();
        }
        else
        {
            TriggerConversation();
        }
    }

    void OpenDoor()
    {
        if (interactable && !isInteracting)
        {
            if (playerHolding.GetLeftHand())
            {
                if (Input.GetMouseButton(0))
                {
                    isInteracting = true;
                    activated = true;
                    cameraControl.TurnOnCamera();
                    leftHandOpen = true;
                }
            }
        }

        if (isInteracting)
        {
            float verticalInput = Input.GetAxis("Mouse X") * Time.deltaTime * 75;
            Debug.Log(verticalInput);
            if (verticalInput < 0)
            {
                StartCoroutine(LerpPosition(doorOpenPos, 2f));
            }

            if (leftHandOpen)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    isInteracting = false;
                }
            }
        }

        if (door.localPosition == doorOpenPos)
        {
            doorOpen = true;
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
        float time = 0;
        Vector3 startPosition = door.transform.localPosition;
        while (time < duration)
        {
            door.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        door.transform.localPosition = targetPosition;
    }
}
