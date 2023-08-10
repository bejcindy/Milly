using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IzakayaDoor : LivableObject
{
    public bool isInteracting;
    public bool leftHandOpen;
    public bool rightHandOpen;
    public bool doorOpen;
    public Transform door;
    public Vector3 doorOpenPos;

    PlayerHolding playerHolding;
    FixedCameraObject cameraControl;
    Vector3 doorStartPos;

    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
        cameraControl = GetComponent<FixedCameraObject>();
        doorStartPos = door.transform.localPosition;
    }
    protected override void Update()
    {
        base.Update();
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
            if(verticalInput < 0)
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

        if(door.localPosition == doorOpenPos)
        {
            doorOpen = true;
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
