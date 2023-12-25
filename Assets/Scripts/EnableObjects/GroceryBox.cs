using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VInspector;

public class GroceryBox : PickUpObject
{
    Collider groBoxCollider;
    public static bool boxActivated;

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
        if(game.questAccepted && !baseCandidate && !boxMoving)
            base.Update();
        else if (baseCandidate)
        {
            gameObject.layer = 9;
        }
        else if(activated)
        {
            gameObject.layer = 17;
        }
        else
        {
            gameObject.layer = 0;
        }

        if (activated && !GroceryBox.boxActivated)
        {
            GroceryBox.boxActivated = true;
        }

        if (GroceryBox.boxActivated && !activated)
        {
            activated = true;
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
            groBoxCollider.enabled = true;
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

    public void CheckPlaceBox()
    {
        if (game.placeableBox != null)
        {
            Vector3 topPos = new Vector3(game.placeableBox.transform.position.x, game.placeableBox.transform.position.y + 1f, game.placeableBox.transform.position.z);
            Quaternion topRot = Quaternion.Euler(new Vector3(0, transform.rotation.y, 0));
            rb.isKinematic = false;
            transform.position = topPos;
            transform.rotation = topRot;
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


}
