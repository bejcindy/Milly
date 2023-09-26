using System.Collections;
using UnityEngine;

public class Door : LivableObject
{
    public Transform door;
    public bool doorOpen;
    public bool doorMoving;
    public bool doorInteractable;
    public bool isInteracting;

    public bool slidingDoor;
    

    PlayerHolding playerHolding;
    Vector3 closedPos;
    public Vector3 openPos;

    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
        if (slidingDoor)
            closedPos = door.localPosition;
        else
            closedPos = door.localEulerAngles;
    }

    protected override void Update()
    {
        base.Update();
        DoorControl();

    }

    protected virtual void DoorControl()
    {
        if (doorInteractable)
        {
            if (playerHolding.GetLeftHand())
            {
                if (Input.GetMouseButton(0) && checkVisible)
                {
                    activated = true;
                    isInteracting = true;
                }
            }

            if (isInteracting && checkVisible)
            {
                float horizontalInput = Input.GetAxis("Mouse X") * Time.deltaTime * 75;
                float verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime * 75;
                if (!doorMoving)
                {
                    if (!doorOpen)
                    {
                        if (slidingDoor)
                        {
                            if (horizontalInput < 0)
                            {
                                StartCoroutine(LerpPosition(openPos, 1f));
                            }
                        }
                        else
                        {
                            if(verticalInput > 0)
                            {
                                StartCoroutine(LerpRotation(Quaternion.Euler(openPos), 2f));
                            }
                        }

                    }
                    else
                    {
                        if (slidingDoor)
                        {
                            if (horizontalInput > 0)
                            {
                                StartCoroutine(LerpPosition(closedPos, 1f));
                            }
                        }
                        else
                        {
                            if (verticalInput < 0)
                            {
                                StartCoroutine(LerpRotation(Quaternion.Euler(closedPos), 2f));
                            }
                        }

                    }
                }

            }


        }
        if (Input.GetMouseButtonUp(0))
        {
            isInteracting = false;
        }

        if (door.localPosition == openPos)
        {
            doorOpen = true;
        }
        else
        {
            doorOpen = false;
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        doorMoving = true;
        float time = 0;
        Vector3 startPosition = door.localPosition;
        while (time < duration)
        {
            door.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        door.localPosition = targetPosition;
        doorMoving = false;
    }

    IEnumerator LerpRotation(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = door.localRotation;
        while (time < duration)
        {
            door.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        door.localRotation = endValue;
    }
}
