using System.Collections;
using UnityEngine;
using FMODUnity;
using PixelCrushers.DialogueSystem;
using VInspector;

public class Door : LivableObject
{
    [Foldout("Door")]
    public Transform door;
    public bool doorOpen;
    public bool doorMoving;
    public bool doorInteractable;
    public bool isInteracting;

    public bool slidingDoor;
    public bool barDoor;
    public bool invertControl;

    public bool playerInFront;
    Vector3 closedPos;
    public Vector3 openPos;
    public Collider doorHandleCollider;
    public EventReference doorMoveEvent;
    public EventReference doorOpenEvent;
    public EventReference doorCloseEvent;

    protected override void Start()
    {
        base.Start();
        doorType = true;
        if (slidingDoor)
        {
            closedPos = door.localPosition;
        }
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
            if (!playerHolding.GetLeftHand())
            {
                playerLeftHand.enabled = false;
            }
            if (!playerHolding.atTable)
            {
                DoorControl();
            }

        }
        else
        {
            if (!playerHolding.GetLeftHand())
                playerLeftHand.enabled = true;


            isInteracting = false;
            if (playerHolding.doorHandle == doorHandleCollider.gameObject)
            {
                playerHolding.doorHandle = null;
            }

        }

        if (slidingDoor)
        {
            if (door.localPosition == openPos)
                doorOpen = true;
            else
                doorOpen = false;
        }
        else
        {
            if (door.localRotation == Quaternion.Euler(openPos))
                doorOpen = true;
            else
                doorOpen = false;
        }
        

    }

    protected virtual void DoorControl()
    {

        if (Input.GetMouseButton(0))
        {
            isInteracting = true;
        }
        playerHolding.doorHandle = doorHandleCollider.gameObject;


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
                                StartCoroutine(LerpPosition(openPos, 1.5f));
                            playerHolding.dragAnimDirection = "Left";
                        }
                        else
                        {
                            if (horizontalInput > 0)
                                StartCoroutine(LerpPosition(openPos, 1.5f));
                            playerHolding.dragAnimDirection = "Right";
                        }
                    }
                    else
                    {
                        if (playerInFront)
                        {
                            if (!invertControl)
                            {
                                if (verticalInput > 0)
                                    StartCoroutine(LerpRotation(Quaternion.Euler(openPos), 2f));
                                playerHolding.dragAnimDirection = "Up";
                            }
                            else
                            {
                                if (verticalInput < 0)
                                    StartCoroutine(LerpRotation(Quaternion.Euler(openPos), 2f));
                                playerHolding.dragAnimDirection = "Down";
                            }

                        }
                        else
                        {
                            if (!invertControl)
                            {
                                if (verticalInput < 0)
                                    StartCoroutine(LerpRotation(Quaternion.Euler(openPos), 2f));
                                playerHolding.dragAnimDirection = "Down";
                            }
                            else
                            {
                                if (verticalInput > 0)
                                    StartCoroutine(LerpRotation(Quaternion.Euler(openPos), 2f));
                                playerHolding.dragAnimDirection = "Up";
                            }

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
                                StartCoroutine(LerpPosition(closedPos, 1.5f));
                            playerHolding.dragAnimDirection = "Right";
                        }
                        else
                        {
                            if (horizontalInput < 0)
                                StartCoroutine(LerpPosition(closedPos, 1.5f));
                            playerHolding.dragAnimDirection = "Left";
                        }
                    }
                    else
                    {
                        if (playerInFront)
                        {
                            if (verticalInput < 0)
                                StartCoroutine(LerpRotation(Quaternion.Euler(closedPos), 2f));
                            playerHolding.dragAnimDirection = "Down";
                        }
                        else
                        {
                            if (verticalInput > 0)
                                StartCoroutine(LerpRotation(Quaternion.Euler(closedPos), 2f));
                            playerHolding.dragAnimDirection = "Up";
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))        
            isInteracting = false;
    }

    public void NPCOpenDoor()
    {
        if (!doorOpen && !doorMoving)
        {
            if(slidingDoor)
                StartCoroutine(LerpPosition(openPos, 1f));
            else
                StartCoroutine(LerpRotation(Quaternion.Euler(openPos), 2f));
        }        
   
    }

    public void CloseDoor()
    {
        if (doorOpen)
        {
            if(slidingDoor)
                StartCoroutine(LerpPosition(closedPos, 1f));
            else
                StartCoroutine(LerpRotation(Quaternion.Euler(closedPos), 2f));
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
        if (!activated)
        {
            activated = true;
        }
        if (!doorMoving)
            RuntimeManager.PlayOneShot(doorMoveEvent, doorHandleCollider.transform.position);
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
        if (!activated)
        {
            activated = true;
        }
        if (!doorMoving)
        {
            if (endValue == Quaternion.Euler(openPos))
                RuntimeManager.PlayOneShot(doorOpenEvent, doorHandleCollider.transform.position);
        }
        doorMoving = true;
        float time = 0;
        Quaternion startValue = door.localRotation;
        while (time < duration)
        {
            door.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        door.localRotation = endValue;
        if (endValue == Quaternion.Euler(closedPos))
            RuntimeManager.PlayOneShot(doorCloseEvent, transform.position);

        doorMoving = false;
    }
}
