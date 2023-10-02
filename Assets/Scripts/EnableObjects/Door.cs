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

    public bool playerInFront;

    PlayerHolding playerHolding;
    Vector3 closedPos;
    public Vector3 openPos;
    public Collider doorHandleCollider;

    bool playedWithDoor;

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
        checkVisible = isDoorHandleVisible(doorHandleCollider);
        Vector3 pointOnScreen = Camera.main.WorldToScreenPoint(doorHandleCollider.bounds.center);
        if (checkVisible)
        {
            if ((pointOnScreen.x > Screen.width * 0.2f) || (pointOnScreen.x < Screen.width * 0.8f) ||
            (pointOnScreen.y > Screen.height * 0.2f) || (pointOnScreen.y < Screen.height * 0.8f))
                isVisible = true;
            else
                isVisible = false;
        }
        else
            isVisible = false;


        playerInFront = CheckPlayerForward();
        if (interactable)
        {
            DoorControl();
            playedWithDoor = true;
        }
        else
        {
            if (playedWithDoor)
            {
                playerHolding.doorHandle = null;
                playedWithDoor = false;
            }

        }
    }

    protected virtual void DoorControl()
    {

        if (playerHolding.GetLeftHand())
        {
            if (Input.GetMouseButton(0))
            {
                activated = true;
                isInteracting = true;
                playedWithDoor = true;
            }
            playerHolding.doorHandle = doorHandleCollider.gameObject;
        }

        if (isInteracting)
        {
            float horizontalInput = Input.GetAxis("Mouse X") * Time.deltaTime ;
            float verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime ;
            if (!doorMoving)
            {
                if (!doorOpen)
                {
                    if (slidingDoor)
                    {
                        if (playerInFront)
                        {
                            if (horizontalInput < 0)
                                StartCoroutine(LerpPosition(openPos, 2f));
                        }
                        else
                        {
                            if (horizontalInput > 0)
                                StartCoroutine(LerpPosition(openPos, 2f));
                        }

                    }
                    else
                    {
                        if (playerInFront)
                        {
                            if (verticalInput > 0)
                                StartCoroutine(LerpRotation(Quaternion.Euler(openPos), 2f));
                        }
                        else
                        {
                            if (verticalInput < 0)
                                StartCoroutine(LerpRotation(Quaternion.Euler(openPos), 2f));

                        }

                    }

                }
                else
                {
                    if (slidingDoor)
                    {
                        if (playerInFront)
                        {
                            if (horizontalInput > 0)
                                StartCoroutine(LerpPosition(closedPos, 2f));
                        }
                        else
                        {
                            if (horizontalInput < 0)
                                StartCoroutine(LerpPosition(closedPos, 2f));

                        }

                    }
                    else
                    {
                        if (playerInFront)
                        {
                            if (verticalInput < 0)
                                StartCoroutine(LerpRotation(Quaternion.Euler(closedPos), 2f));
                        }
                        else
                        {
                            if (verticalInput > 0)
                                StartCoroutine(LerpRotation(Quaternion.Euler(closedPos), 2f));
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



    public void NPCOpenDoor()
    {
        if (!doorOpen)
        {
            StartCoroutine(LerpPosition(openPos, 1f));
        }
    }

    public void CloseDoor()
    {
        if (doorOpen)
        {
            StartCoroutine(LerpPosition(closedPos, 1f));
        }
    }

    bool CheckPlayerForward()
    {
        float angle = Vector3.Angle(transform.forward, player.position - transform.position);
        if (Mathf.Abs(angle) < 90)
            return true;
        return false;
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
