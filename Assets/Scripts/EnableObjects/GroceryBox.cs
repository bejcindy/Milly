using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class GroceryBox : PickUpObject
{
    Collider groBoxCollider;

    [Foldout("GroceryBox")]
    public bool baseCandidate;
    public int stackCount;
    public float upDetectDist;
    public bool boxAbove;
    public bool boxBelow;
    public bool groundBox;
    public bool boxForceMove;
    public LayerMask flatGround;
    [SerializeField] GroceryBoxGame game;
    GroceryBox upperBox, lowerBox;
    bool boxMoving;
    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.DOUBLE;
        groBoxCollider = GetComponent<Collider>();
    }

    protected override void Update()
    {
        if(game.questAccepted && !boxMoving)
            base.Update();
        else if (boxMoving)
        {
            selected = false;
        }


        if (inHand)
        {
            groBoxCollider.enabled = false;
            boxAbove = false;
            boxBelow = false;
            groundBox = false;
        }
        else
        {
            //if box is out of hand and lerping or when box is just not in hand, then collider is activate
            if(!boxForceMove)
                groBoxCollider.enabled = true;

            //if a box on the floor is interactable and player is holding a box, then this one is detectable by playerholding
            if(CheckPlayerHoldingBox() && interactable)
            {
                baseCandidate = true;
                playerHolding.AddInteractable(gameObject);
            }
            else
            {
                baseCandidate = false;
            }

            //if this box on the floor is selected and no box is lerping
            if(baseCandidate && selected && !boxMoving)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    playerHolding.RemoveInteractable(gameObject);
                    selected = false;
                    GroceryBox moveBox = playerLeftHand.holdingObj.GetComponent<GroceryBox>();
                    moveBox.transform.SetParent(null);
                    Vector3 topPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                    StartCoroutine(LerpPosition(moveBox.transform, topPos, 1f));
                    StartCoroutine(LerpRotation(moveBox.transform, Quaternion.Euler(Vector3.zero), 0.5f));

                    playerLeftHand.isHolding = false;
                    moveBox.transform.GetComponent<PickUpObject>().inHand = false;
                    moveBox.transform.GetComponent<PickUpObject>().thrown = true;
                    moveBox.transform.GetComponent<PickUpObject>().thrownByPlayer = true;
                    playerHolding.UnoccupyLeft();


                }
            }
        }
    }

    public void FixedUpdate()
    {
        if (game.inGameZone && !rb.isKinematic)
        {
            BoxCollisionDetection();
            groundBox = Physics.Raycast(transform.position, Vector3.down, upDetectDist, flatGround);
            
            if(game.gameStarted) 
                stackCount = CalculateBoxChain(1, this);
        }
        else
        {
            boxAbove = false;
            boxBelow = false;
            groundBox = false;
        }
    }

    bool CheckPlayerHoldingBox()
    {
        if(!playerHolding.GetLeftHand() && playerLeftHand.holdingObj.GetComponent<GroceryBox>())
        {
            return true;
        }
        return false;
    }


    void BoxCollisionDetection()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, upDetectDist))
        {
            if (hit.collider.gameObject.CompareTag("GroceryBox"))
            {
                boxAbove = true;
                upperBox = hit.collider.gameObject.GetComponent<GroceryBox>();
            }

            else
            {
                boxAbove = false;
                upperBox = null;
            }


        }
        else
        {
            boxAbove = false;
            upperBox = null;
        }

        if (Physics.Raycast(transform.position, Vector3.down, out hit, upDetectDist))
        {
            if (hit.collider.gameObject.CompareTag("GroceryBox"))
            {
                if (!boxAbove && !boxBelow && !groundBox)
                    game.gameStarted = true;
                boxBelow = true;
                lowerBox = hit.collider.gameObject.GetComponent<GroceryBox>();
            }
            else
            {
                boxBelow = false;
                lowerBox = null;
            }

        }
        else
        {
            boxBelow = false;
            lowerBox = null;
        }
    }

    public int CalculateBoxChain(int count, GroceryBox box)
    {
        if (!box.boxAbove)
        {
            return count;
        }
        else
        {
            return (CalculateBoxChain(1+count, box.upperBox));
        }
    }


    IEnumerator LerpPosition(Transform box, Vector3 targetPosition, float duration)
    {
        box.GetComponent<GroceryBox>().boxForceMove = true;
        boxMoving = true;
        float time = 0;
        Vector3 startPosition = box.position;
        while (time < duration)
        {
            box.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        box.position = targetPosition;
        boxMoving = false;
        box.GetComponent<GroceryBox>().boxForceMove = false;
        box.GetComponent<Rigidbody>().isKinematic = false;

    }

    IEnumerator LerpRotation(Transform box, Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = box.rotation;
        while (time < duration)
        {
            box.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        box.rotation = endValue;
    }
}
