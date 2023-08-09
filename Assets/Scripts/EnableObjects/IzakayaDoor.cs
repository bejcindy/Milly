using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IzakayaDoor : LivableObject
{
    public bool isInteracting;
    public bool leftHandOpen;
    public bool rightHandOpen;
    public Transform door;

    PlayerHolding playerHolding;
    FixedCameraObject cameraControl;

    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
        cameraControl = GetComponent<FixedCameraObject>();
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
            float verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime * 75;
            if(verticalInput > 0)
            {
                door.position -= Vector3.right * verticalInput * Time.deltaTime;
            }

            if (leftHandOpen)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    isInteracting = false;
                }
            }
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.localPosition;
        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition;
    }
}
